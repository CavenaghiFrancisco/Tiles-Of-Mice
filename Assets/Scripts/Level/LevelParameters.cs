using UnityEngine;
using TOM.Enemy;

namespace TOM
{
    [CreateAssetMenu(fileName = "Level Template", menuName = "Level/New Level")]
    public class LevelParameters : ScriptableObject
    {
        [Header("Level ID")]
        public int levelID;

        [Header("CyberRoach")]
        public EnemyParameters CRParams;
        public int CyberRoackCount;
        
        [Header("SV-23")]
        public EnemyParameters SVParams;
        public int SV23Count;

        [Header("Gregorio")]
        public EnemyParameters GregParams;
        public bool spawnGregorio;
    }
}