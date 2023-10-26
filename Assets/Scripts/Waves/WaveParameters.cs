using UnityEngine;

namespace TOM
{
    [CreateAssetMenu(fileName = "Wave ", menuName = "Level/Wave Parameters")]
    public class WaveParameters : ScriptableObject
    {
        //ID interno  Enemy Threshold Enemy Level Enemy Quantity  Enemy Delay
        [Header("Wave ID")]
        public int waveID;
        [Header("Enemy Threshold")]
        public int waveThreshold;
        [Header("Enemy Level")]
        public int waveLevel;
        [Header("Enemy Quantity")]
        public int enemyAmount; // Si agregamos otro enemigo vamos a tener que cambiar esto.
        [Header("Enemy Delay (Seconds)")]
        public float enemyDelay;
    }
}