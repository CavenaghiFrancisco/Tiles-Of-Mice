using UnityEngine;
namespace TOM
{
    public abstract class Entity : MonoBehaviour
    {
        protected int hp;
        protected int basicAtk;
        protected int powerAtk;
        protected int moveSpeed;//Aplicar a br1e
        protected float basicHitCD;
        protected float powerHitCD;
        protected bool isAlive;
        protected float hurtTime;

        protected System.Action OnDeath;
        public abstract void GetDamage(int damage);
        public abstract void Die();
        public abstract void Attack(Entity otherEntity);
        protected abstract void EntityReset();
        protected void OnTriggerEnter(Collider other)
        {
            if (isAlive)
            {
                if (other.tag == "DeathZone")
                {
                    //Desconectar la camara de brie
                    Die();
                }
            }
        }
        public int GetMovementSpeed() => moveSpeed;
        public bool IsAlive() => isAlive;
        public float GetBasicHitCD() => basicHitCD;
        public float GetPowerHitCD() => powerHitCD;
    }
}