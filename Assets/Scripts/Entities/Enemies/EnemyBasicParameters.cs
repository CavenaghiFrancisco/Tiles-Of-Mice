using UnityEngine;

namespace TOM.Enemy
{
    [CreateAssetMenu(fileName = "Enemy Basic Parameters", menuName = "Entities/Enemy Basic Parameters")]
    public class EnemyBasicParameters : ScriptableObject
    {
        public int healthPoints;
        public int basicAttack;
        public float basicAttackCoolDown;
        public int powerAttack;
        public float powerAttackCoolDown;
        public int powerAttackChance;
        public float attackRadius;
        public float stunTimeInms;
        public int movementSpeed;
        public float hurtTime;
    }
}