using IA.FSM;
using UnityEngine;

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

        private float distance;
        private GameObject player;

        private void Update()
        {
            if(player == null)
            {
                player = GameObject.FindGameObjectWithTag("Player");
            }
            if (player != null)
            {
                distance = Vector3.Distance(this.gameObject.transform.position,player.transform.position);
                AkSoundEngine.SetRTPCValue("Distance", distance, this.gameObject);
            }
            
        }
    }
}
