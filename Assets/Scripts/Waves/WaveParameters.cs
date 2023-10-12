using UnityEngine;

namespace TOM
{
    [CreateAssetMenu(fileName = "Wave ", menuName = "Level/Wave Parameters")]
    public class WaveParameters : ScriptableObject
    {
        //ID interno  Enemy Threshold Enemy Level Enemy Quantity  Enemy Delay
        [Header("Wave ID")]
        public float waveID;
        [Header("Enemy Threshold")]
        public float waveThreshold;
        [Header("Enemy Level")]
        public float waveLevel;
        [Header("Enemy Quantity")]
        public float enemyAmount; // Si agregamos otro enemigo vamos a tener que cambiar esto.
        [Header("Enemy Delay")]
        public float enemyDelay;
    }
}