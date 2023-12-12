using UnityEngine;

namespace TOM.Enemy
{
    [CreateAssetMenu(fileName = "Enemy Grow Parameters", menuName = "Entities/Enemy Grow Parameters")]
    public class EnemyGrowParameters : ScriptableObject
    {
        [Header("HP x lvl")]
        public float growingHealthPoints;
        [Header("Basic Attack x lvl")]
        public float growingBasicAttack;
        [Header("Power Attack x lvl")]
        public float growingPowerAttack;
        [Header("Movement Speed x lvl")]
        public float growingMovementSpeed;
    }
}