using System.Collections.Generic;
using System.Collections;
using TOM.Utilities;
using TOM.Enemy.CR;
using TOM.Enemy.TR;
using UnityEngine;

namespace TOM.Enemy
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] private GameObject player = null;

        [SerializeField] private Transform arenaCenter = null;

        [SerializeField] private Transform spawnTransform = null;

        [SerializeField] private Transform bulletFolder = null;

        [SerializeField] private EnemySpecs enemySpecs = default;

        private int actualLevelID = 0;
        private int maxCRCount = 0;
        private int maxTRCount = 0;

        private int CRSpawnAmount = 0;
        private int TRSpawnAmount = 0;

        private int killedCRs = 0;
        private int killedTRs = 0;

        private int createdCRs = 1;
        private int createdTRs = 1;

        private Transform CRFolder;
        private Transform TRFolder;

        private bool everyCRCreated = false;
        private bool everyTRCreated = false;

        //private Transform GregorioFolder;

        private List<CyberRoach> CyberRoachList = new List<CyberRoach>();
        private List<CyberRoachBehavior> CyberRoachBehaviorList = new List<CyberRoachBehavior>();

        private List<ToxicRoach> ToxicRoachList = new List<ToxicRoach>();
        private List<ToxicRoachBehavior> ToxicRoachBehaviorList = new List<ToxicRoachBehavior>();

        public static System.Action OnAllEnemiesCreated;
        public System.Action OnAllEnemiesKilled;

        private ObjectPool<GameObject> crPool;
        private ObjectPool<GameObject> trPool;

        private void Awake()
        {
            //maxCRCount = levelList.OrderByDescending(item => item.CyberRoackCount).First().CyberRoackCount;//Cantidad de CR
            //maxSVCount = levelList.OrderByDescending(item => item.SV23Count).First().SV23Count;//Cantidad de SV
            //levelList = levelList.OrderBy(x => x.levelID).ToList();//Ordeno la lista por IDs

            CyberRoachBehavior.SetArenaCenter(arenaCenter.position);//Setteo el centro de la arena al cual las cyberroachs van a ir cuando spawneen.

            crPool = new ObjectPool<GameObject>(enemySpecs.CRPrefab);
            trPool = new ObjectPool<GameObject>(enemySpecs.TRPrefab);

            CreateFolders();

        }

        private void OnDestroy()
        {
            foreach (CyberRoach cr in CyberRoachList)
            {
                cr.OnDeath -= CRKillCounter;
            }
            foreach (ToxicRoach tr in ToxicRoachList)
            {
                tr.OnDeath -= TRKillCounter;
            }
        }

        public void TurnOnEnemiesOnLevel(int enemyLevel, int CRamount, int TRamount, float delay)
        {
            this.enemySpecs = enemySpecs;

            CRSpawnAmount = CRamount;
            killedCRs = 0;

            TRSpawnAmount = TRamount;
            killedTRs = 0;

            CreateEnemies(enemyLevel, CRamount, TRamount, delay);
        }
        private void CreateFolders()
        {
            GameObject auxGo = new GameObject("CyberRoach List");
            auxGo.transform.SetParent(this.transform);
            auxGo.transform.position = spawnTransform.position;
            CRFolder = auxGo.transform;

            auxGo = new GameObject("ToxicRoach List");
            auxGo.transform.SetParent(this.transform);
            TRFolder = auxGo.transform;
        }

        private void CreateEnemies(int waveLevel, int crAmount, int trAmount, float delayInSeconds)
        {
            StartCoroutine(CreateCyberRoachs(waveLevel, crAmount, delayInSeconds));
            StartCoroutine(CreateToxicRoachs(waveLevel, trAmount, delayInSeconds));
        }

        private void EnableCyberRoach(int waveLevel)
        {
            GameObject thisCR = crPool.GetAbleObject(OPType.CyberRoach);

            CyberRoach cr;
            CyberRoachBehavior crb;

            if (thisCR.name.Length == 4)
            {
                thisCR = Instantiate(crPool.GetT(), CRFolder);
                thisCR.name = "CR - " + createdCRs.ToString();
                //Debug.Log("Cree a "+thisCR.name);

                cr = thisCR.AddComponent<CyberRoach>();
                crb = thisCR.AddComponent<CyberRoachBehavior>();

                CyberRoachList.Add(cr);
                CyberRoachBehaviorList.Add(crb);

                cr.Initialize(enemySpecs.CRBasicParams, enemySpecs.CRGrowParams);
                cr.OnDeath += CRKillCounter;

                crb.Initialize(player);

                createdCRs++;
            }
            else
            {
                cr = thisCR.GetComponent<CyberRoach>();

                crb = thisCR.GetComponent<CyberRoachBehavior>();
                crb.ResetBehaviour();

                cr.gameObject.SetActive(true);

                //Debug.Log("Actualice a " + thisCR.name);
            }

            cr.Grow(waveLevel);

            cr.gameObject.transform.position = GetRandomPosition(spawnTransform.position);
            cr.gameObject.GetComponent<Rigidbody>().position = thisCR.transform.position;

            StartCoroutine(WaitFrame(thisCR));

            crPool.UpdateLastGivenObject(thisCR);
        }

        private void EnableToxicRoach(int waveLevel)
        {
            GameObject thisTR = trPool.GetAbleObject(OPType.ToxicRoach);

            ToxicRoach tr;
            ToxicRoachBehavior trb;

            if (thisTR.name.Length == 4)
            {
                thisTR = Instantiate(trPool.GetT(), TRFolder);
                thisTR.name = "TR - " + createdTRs.ToString();
                //Debug.Log("Cree a "+thisCR.name);

                tr = thisTR.AddComponent<ToxicRoach>();
                trb = thisTR.AddComponent<ToxicRoachBehavior>();

                ToxicRoachList.Add(tr);
                ToxicRoachBehaviorList.Add(trb);

                tr.Initialize(enemySpecs.TRBasicParams, enemySpecs.TRGrowParams, enemySpecs.bulletPrefab, bulletFolder);
                tr.OnDeath += TRKillCounter;

                trb.Initialize(player);

                createdTRs++;
            }
            else
            {
                tr = thisTR.GetComponent<ToxicRoach>();

                trb = thisTR.GetComponent<ToxicRoachBehavior>();
                trb.ResetBehaviour();

                tr.gameObject.SetActive(true);

                //Debug.Log("Actualice a " + thisCR.name);
            }

            tr.Grow(waveLevel);

            tr.gameObject.transform.position = GetRandomPosition(spawnTransform.position);
            tr.gameObject.GetComponent<Rigidbody>().position = thisTR.transform.position;

            StartCoroutine(WaitFrame(thisTR));

            trPool.UpdateLastGivenObject(thisTR);
        }

        private IEnumerator WaitFrame(GameObject enemy)
        {
            yield return new WaitForEndOfFrame();
            enemy.SetActive(true);
        }

        private IEnumerator CreateCyberRoachs(int waveLevel, int amount, float delayInSeconds)
        {
            int createdAmount = 0;
            while (createdAmount < amount)
            {
                EnableCyberRoach(waveLevel);
                createdAmount++;
                yield return new WaitForSeconds(delayInSeconds);
            }

            everyCRCreated = true;
            CheckEveryEnemyCreated();
        }

        private IEnumerator CreateToxicRoachs(int waveLevel, int amount, float delayInSeconds)
        {
            int createdAmount = 0;
            while (createdAmount < amount)
            {
                EnableToxicRoach(waveLevel);
                createdAmount++;
                yield return new WaitForSeconds(delayInSeconds);
            }

            everyTRCreated = true;

            CheckEveryEnemyCreated();

        }

        private void CheckEveryEnemyCreated()
        {
            if (everyCRCreated && everyTRCreated)
                OnAllEnemiesCreated?.Invoke();
        }


        private Vector3 GetRandomPosition(Vector3 center)
        {
            Bounds bounds = spawnTransform.GetComponent<BoxCollider>().bounds;
            Vector3 offset = Vector3.zero;
            offset.x = Random.Range(-bounds.extents.x, bounds.extents.x);
            offset.z = Random.Range(-bounds.extents.z, bounds.extents.z);
            return center + offset;
        }

        public void KillEnemies()
        {
            foreach (CyberRoach enemy in CyberRoachList)
            {
                enemy.Die();
            }
            foreach (ToxicRoach enemy in ToxicRoachList)
            {
                enemy.Die();
            }
        }
        private void CRKillCounter()
        {
            killedCRs++;
            Debug.Log("Kill Counter: " + killedCRs + "/" + CRSpawnAmount);
            CheckKillCounter();
        }
        private void TRKillCounter()
        {
            killedTRs++;
            Debug.Log("Kill Counter: " + killedTRs + "/" + TRSpawnAmount);
            CheckKillCounter();
        }

        private void CheckKillCounter()
        {
            if (killedCRs + killedTRs == CRSpawnAmount + TRSpawnAmount)
            {
                Debug.Log("Suficiente para pasar de nivel...");
                OnAllEnemiesKilled?.Invoke();
                //fameManager.GetFame();
            }
        }
    }
}