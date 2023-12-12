using UnityEngine;

namespace TOM
{
    [CreateAssetMenu(fileName = "Fame Scalation 1", menuName = "Level/Fame Parameters")]
    public class FameParameters : ScriptableObject
    {
        [Header("Fame Obtain")]
        public int baseFameObtained;
        [Header("Fame Multiplier")]
        public float fameGivenMultiplier;
        [Header("Time Options (In seconds)")]
        public int levelSafeTime;
        public int levelLosingTime;
    }
}