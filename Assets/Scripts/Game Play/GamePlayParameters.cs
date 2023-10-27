using UnityEngine;
using System.Collections.Generic;
using TOM.Enemy;

namespace TOM
{
    [CreateAssetMenu(fileName = "Level Template", menuName = "Level/New Level")]
    public class GamePlayParameters : ScriptableObject
    {
        [Header("Level ID")]
        public int levelID;

        [Header("CyberRoach")]
        public EnemyBasicParameters CRBasicParams;
        public EnemyGrowParameters CRGrowParams;

        [Header("Waves")]
        public int levelAugmentAmount;
        public List<WaveParameters> waveList;


        //[Header("SV-23")]
        //public EnemyBasicParameters SVBasicParams;
        //public EnemyGrowParameters SVGrowParams;

        //[Header("Gregorio")]
        //public EnemyBasicParameters GregBasicParams;
        //public EnemyGrowParameters GregGrowParams;
    }
}