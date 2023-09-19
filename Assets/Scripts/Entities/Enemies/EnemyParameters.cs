using UnityEngine;

namespace TOM.Enemy
{
    [CreateAssetMenu(fileName = "Enemy Template", menuName = "Entities/Enemy")]
    public class EnemyParameters : ScriptableObject
    {
        public int healthPoints;
        public int basicAttack;
        public float basicAttackCoolDown;
        public int powerAttack;
        public float powerAttackCoolDown;
        public int powerAttackChance;
        public int attackRadius;
        public float stunTimeInms;
        public int movementSpeed;
        public float hurtTime;
    }
}