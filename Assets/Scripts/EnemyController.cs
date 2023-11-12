using System.Collections;
using System.Collections.Generic;
using System.Configuration.Assemblies;
using UnityEngine;
using System.Linq;
using TOM.Enemy.CR;
using TOM.Utilities;

namespace TOM.Enemy
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] private GameObject player = null;

        [SerializeField] private EnemySpecs enemySpecifications = null;

        [SerializeField] private Transform arenaCenter = null;

        [SerializeField] private Transform spawnTransform = null;

        private int actualLevelID = 0;
        private int maxCRCount = 0;
        private int maxSVCount = 0;

        private int enemyThreshold = 0;
        private int enemySpawnAmount = 0;
        private int killedEnemies = 0;

        private int createdEnemies = 1;

        private Transform CRFolder;
        private Transform SVFolder;
        private Transform GregorioFolder;

        private List<CyberRoach> CyberRoachList = new List<CyberRoach>();
        private List<CyberRoachBehavior> CyberRoachBehaviorList = new List<CyberRoachBehavior>();
        //private List<CyberRoach> CyberRoachList = new List<CyberRoach>();

        public static System.Action OnAllEnemiesCreated;
        public System.Action OnEnemyThreshold;

        private ObjectPool<GameObject> crPool;

        private void Awake()
        {
            //maxCRCount = levelList.OrderByDescending(item => item.CyberRoackCount).First().CyberRoackCount;//Cantidad de CR
            //maxSVCount = levelList.OrderByDescending(item => item.SV23Count).First().SV23Count;//Cantidad de SV
            //levelList = levelList.OrderBy(x => x.levelID).ToList();//Ordeno la lista por IDs

            CyberRoachBehavior.SetArenaCenter(arenaCenter.position);//Setteo el centro de la arena al cual las cyberroachs van a ir cuando spawneen.

            crPool = new ObjectPool<GameObject>(enemySpecifications.CRPrefab);

            CreateFolders();

        }

        private void OnDestroy()
        {
            foreach (CyberRoach cr in CyberRoachList)
            {
                cr.OnDeath -= EnemyKillCounter;
            }
        }

        public void TurnOnEnemiesOnLevel(int enemyLevel, int amount, float delay, int threshold)
        {
            enemySpawnAmount = amount;
            enemyThreshold = threshold;
            killedEnemies = 0;
            CreateEnemies(enemyLevel, amount, delay);
        }
        private void CreateFolders()
        {
            GameObject auxGo = new GameObject("CyberRoach List");
            auxGo.transform.SetParent(this.transform);
            auxGo.transform.position = spawnTransform.position;
            CRFolder = auxGo.transform;

            auxGo = new GameObject("SV-23 List");
            auxGo.transform.SetParent(this.transform);
            SVFolder = auxGo.transform;
        }

        private void CreateEnemies(int waveLevel, int crAmount, float delayInSeconds)
        {
            StartCoroutine(CreateCyberRoachs(waveLevel, crAmount, delayInSeconds));
        }

        private void EnableCyberRoach(int waveLevel)
        {
            GameObject thisCR = crPool.GetAbleObject();

            CyberRoach cr;
            CyberRoachBehavior crb;
            
            if (thisCR.name.Length == 4)
            {
                thisCR = Instantiate(crPool.GetT(), CRFolder);
                thisCR.name = "CR - " + createdEnemies.ToString();
                //Debug.Log("Cree a "+thisCR.name);

                cr = thisCR.AddComponent<CyberRoach>();
                crb = thisCR.AddComponent<CyberRoachBehavior>();

                CyberRoachList.Add(cr);
                CyberRoachBehaviorList.Add(crb);

                cr.Initialize(enemySpecifications.CRBasicParams, enemySpecifications.CRGrowParams);
                cr.OnDeath += EnemyKillCounter;

                crb.Initialize(player);

                createdEnemies++;
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

        private IEnumerator WaitFrame(GameObject thisCR)
        {
            yield return new WaitForEndOfFrame();
            thisCR.SetActive(true);
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

        private void CreateSV23(int wave, int amount)
        {
            /*Si es que hay, lo dejo para despues*/
        }

        public void KillEnemies()
        {
            foreach (CyberRoach enemy in CyberRoachList)
            {
                enemy.Die();
            }
        }

        private void EnemyKillCounter()
        {
            killedEnemies++;
            //Debug.Log("Kill Counter: " + killedEnemies + "/" + enemySpawnAmount);
            if (killedEnemies == enemySpawnAmount - enemyThreshold)
            {
                Debug.Log("Suficiente para pasar de nivel...");
                OnEnemyThreshold();
                //fameManager.GetFame();
            }
        }

    }
}