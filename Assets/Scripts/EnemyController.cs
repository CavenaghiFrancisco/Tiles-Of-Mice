using System.Collections;
using System.Collections.Generic;
using System.Configuration.Assemblies;
using UnityEngine;
using System.Linq;
using TOM.Enemy.CR;

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

        private void Awake()
        {
            //maxCRCount = levelList.OrderByDescending(item => item.CyberRoackCount).First().CyberRoackCount;//Cantidad de CR
            //maxSVCount = levelList.OrderByDescending(item => item.SV23Count).First().SV23Count;//Cantidad de SV
            //levelList = levelList.OrderBy(x => x.levelID).ToList();//Ordeno la lista por IDs

            CyberRoachBehavior.SetArenaCenter(arenaCenter.position);//Setteo el centro de la arena al cual las cyberroachs van a ir cuando spawneen.

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

        private IEnumerator CreateCyberRoachs(int waveLevel, int amount, float delayInSeconds)
        {
            int alreadyCreated = 0; //Guardo la cantidad que se crearon por si necesito crear mas a medida de que se necesitan

            if (CyberRoachList.Count > 0)
            {
                for (int i = 0; i < CyberRoachList.Count; i++)
                {
                    if (!CyberRoachList[i].gameObject.activeInHierarchy)
                    {
                        CyberRoach cr = CyberRoachList[i];
                        CyberRoachBehavior crb = CyberRoachBehaviorList[i];

                        cr.transform.position = GetRandomPosition(spawnTransform.position);
                        cr.Grow(waveLevel);
                        cr.gameObject.SetActive(true);

                        crb.ResetBehaviour();

                        alreadyCreated++;
                        yield return new WaitForSeconds(delayInSeconds);
                    }
                }
            }

            for (int i = alreadyCreated; i < amount; i++)
            {
                GameObject aux = Instantiate(enemySpecifications.CRPrefab, CRFolder);
                aux.transform.position = GetRandomPosition(spawnTransform.position);
                aux.GetComponent<Rigidbody>().position = aux.transform.position;
                aux.SetActive(true);
                aux.name = "CR - " + createdEnemies.ToString();

                CyberRoach cr = aux.AddComponent<CyberRoach>();
                CyberRoachBehavior crb = aux.AddComponent<CyberRoachBehavior>();

                CyberRoachList.Add(cr);
                CyberRoachBehaviorList.Add(crb);

                cr.gameObject.SetActive(true);
                cr.Initialize(enemySpecifications.CRBasicParams, enemySpecifications.CRGrowParams);
                cr.Grow(waveLevel);
                cr.OnDeath += EnemyKillCounter;

                crb.Initialize(player);

                alreadyCreated++;
                createdEnemies++;
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
            Debug.Log("Kill Counter: " + killedEnemies + "/" + enemySpawnAmount);
            if (killedEnemies == enemySpawnAmount - enemyThreshold)
            {
                Debug.Log("Suficiente para pasar de nivel...");
                OnEnemyThreshold();
            }
        }

    }
}