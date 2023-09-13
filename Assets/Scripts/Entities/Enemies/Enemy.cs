using IA.FSM;

namespace TOM.Enemy
{
    public enum EnemyType
    {
        Normal,
        MiniBoss,
        Boss
    }

    public abstract class Enemy : Entity
    {
        protected float powerHitChance;
        protected FSM fsm;
        protected EnemyType type;
        protected int attackRadius;
        public FSM GetFSM() => fsm;
        public int GetAttackRadius() => attackRadius;
    }
}
