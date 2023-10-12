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

        [SerializeField] private GameObject CRPrefab = null;
        //[SerializeField] private GameObject SVPrefab = null;
        //[SerializeField] private GameObject GregPrefab = null;

        //[SerializeField] private List<LevelParameters> levelList = new List<LevelParameters>();

        private int actualLevelID = 0;
        private int maxCRCount = 0;
        private int maxSVCount = 0;

        private Transform CRFolder;
        private Transform SVFolder;
        private Transform GregorioFolder;

        private List<CyberRoach> CyberRoachList = new List<CyberRoach>();
        private List<CyberRoachBehavior> CyberRoachBehaviorList = new List<CyberRoachBehavior>();
        //private List<CyberRoach> CyberRoachList = new List<CyberRoach>();

        public static System.Action OnAllEnemiesCreated;

        private void Awake()
        {
            //maxCRCount = levelList.OrderByDescending(item => item.CyberRoackCount).First().CyberRoackCount;//Cantidad de CR
            //maxSVCount = levelList.OrderByDescending(item => item.SV23Count).First().SV23Count;//Cantidad de SV
            //levelList = levelList.OrderBy(x => x.levelID).ToList();//Ordeno la lista por IDs

            CreateFolders();
            CreateEnemies();
            
        }

        public void TurnOnLevel(WaveParameters actualWave)
        {
            /*
            if (levelList[levelId].CyberRoackCount>0)
            {
                for (int i = 0; i < levelList[levelId].CyberRoackCount; i++)
                {
                    CyberRoachList[i].Initialize(levelList[levelId].CRParams);
                    CyberRoachList[i].gameObject.SetActive(true);
                }
                OnAllEnemiesCreated?.Invoke();
            }
            */
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

        private void CreateEnemies()
        {
            CreateCyberRoachs();
            CreateSV23();
        }

        private void CreateCyberRoachs()
        {
            for (int i = 0; i < maxCRCount; i++)
            {
                GameObject aux = Instantiate(CRPrefab, CRFolder);
                aux.SetActive(false);
                aux.AddComponent<CyberRoach>();
                aux.AddComponent<CyberRoachBehavior>();
                
                CyberRoachList.Add(aux.GetComponent<CyberRoach>());
                CyberRoachBehaviorList.Add(aux.GetComponent<CyberRoachBehavior>());
                CyberRoachBehaviorList[i].Initialize(player);

            }
        }
        private void CreateSV23()
        {
            /*Si es que hay, lo dejo para despues*/
        }

    }
}