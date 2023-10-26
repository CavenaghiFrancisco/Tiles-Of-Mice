using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TOM.Enemy.CR;

namespace TOM.Enemy
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] private GameObject player = null;

        [SerializeField] private EnemySpecs enemySpecifications = null;

        [SerializeField] private Vector3 arenaCenter = Vector3.zero;
        //[SerializeField] private GameObject SVPrefab = null;
        //[SerializeField] private GameObject GregPrefab = null;

        //[SerializeField] private List<LevelParameters> levelList = new List<LevelParameters>();

        private int actualLevelID = 0;
        private int maxCRCount = 0;
        private int maxSVCount = 0;

        private int enemyThreshold = 0;
        private int enemySpawnAmount = 0;
        private int killedEnemies = 0;

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

            CyberRoachBehavior.SetArenaCenter(arenaCenter);//Setteo el centro de la arena al cual las cyberroachs van a ir cuando spawneen.

            CreateFolders();

        }

        private void OnDestroy()
        {
            foreach (CyberRoach cr in CyberRoachList)
            {
                cr.OnDeath -= EnemyKillCounter;
            }
        }


        public void TurnOnEnemiesOnLevel(int waveLevel, int amount, float delay, int threshold)
        {
            enemySpawnAmount = amount;
            enemyThreshold = threshold;
            killedEnemies = 0;
            CreateEnemies(waveLevel, amount, delay);
        }
        private void CreateFolders()
        {
            GameObject auxGo = new GameObject("CyberRoach List");
            auxGo.transform.SetParent(this.transform);
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

            if (CyberRoachList.Count>0)
            {
                for (int i = 0; i < CyberRoachList.Count; i++)
                {
                    if (!CyberRoachList[i].gameObject.activeInHierarchy)
                    {
                        CyberRoachList[i].gameObject.SetActive(true);
                        CyberRoachList[i].Grow(waveLevel);
                        CyberRoachList[i].OnDeath += EnemyKillCounter;
                        alreadyCreated++;
                        yield return new WaitForSeconds(delayInSeconds);
                    }
                }
            }

            for (int i = alreadyCreated; i < amount; i++)
            {
                GameObject aux = Instantiate(enemySpecifications.CRPrefab, CRFolder);
                aux.SetActive(true);

                CyberRoach a = aux.AddComponent<CyberRoach>();
                CyberRoachBehavior b = aux.AddComponent<CyberRoachBehavior>();

                CyberRoachList.Add(a);

                a.gameObject.SetActive(true);
                a.Initialize(enemySpecifications.CRBasicParams, enemySpecifications.CRGrowParams);
                a.Grow(waveLevel);
                a.OnDeath += EnemyKillCounter;

                CyberRoachBehaviorList.Add(b);
                b.Initialize(player);
                alreadyCreated++;
                yield return new WaitForSeconds(delayInSeconds);
            }

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
            if (killedEnemies == enemySpawnAmount - enemyThreshold)
            {
                OnEnemyThreshold();
            }
        }

    }
}