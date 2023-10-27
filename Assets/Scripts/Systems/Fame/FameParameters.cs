using UnityEngine;

namespace TOM
{
    [CreateAssetMenu(fileName = "Fame Scalation 1", menuName = "Level/Fame Parameters")]
    public class FameParameters : ScriptableObject
    {
        [Header("Fame Obtain")]
        public int fameRotationMultiplier;
        [Header("Fame Needed")]
        public int fameNeededBase;
        public int fameNeededMultiplier;
    }
}