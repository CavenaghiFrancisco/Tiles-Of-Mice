using UnityEngine;
using System.Collections.Generic;
using TOM.Enemy;

namespace TOM
{
    [CreateAssetMenu(fileName = "Enemy Specifications", menuName = "Entities/Enemies/Specifications")]
    public class EnemySpecs : ScriptableObject
    {
        [Header("CyberRoach")] 
        public GameObject CRPrefab;
        public EnemyBasicParameters CRBasicParams;
        public EnemyGrowParameters CRGrowParams;

        //[Header("SV-23")]
        // public GameObject SVPrefab;
        //public EnemyBasicParameters SVBasicParams;
        //public EnemyGrowParameters SVGrowParams;

        //[Header("Gregorio")]
        // public GameObject GregorioPrefab;
        //public EnemyBasicParameters GregBasicParams;
        //public EnemyGrowParameters GregGrowParams;
    }
}