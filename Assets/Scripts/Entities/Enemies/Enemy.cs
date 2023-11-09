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
        protected int powerHitChance;
        protected FSM fsm;
        protected EnemyType type;
        protected float attackRadius;
        protected float stunTimeInms;
        protected bool vulnerable = false;
        public FSM GetFSM() => fsm;
        public float GetAttackRadius() => attackRadius;
        public float GetStunTime() => stunTimeInms;
        public bool IsVulnerable() => vulnerable;
        public void SetVulnerable(bool vul) => vulnerable = vul;
        public abstract void Grow(int wave);
    }
}
