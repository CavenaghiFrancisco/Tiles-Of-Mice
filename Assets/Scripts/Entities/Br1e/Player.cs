using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TOM
{
    public class Player : Entity
    {
        private void Awake() => EntityReset();

        public override void Attack(Entity otherEntity)
        {
            if (isAlive)
            {
                otherEntity.GetDamage(this.basicAtk);
            }
        }

        public override void Die()
        {
            if (isAlive)
            {
                hp = 0;
                //animacion de muerte
                //sfx de muerte
                Debug.Log("Br1e ha muerto");
                gameObject.SetActive(false); //Hacer esto una vez que se ejecuta la animacion de muerte
            }
        }

        public override void GetDamage(int damage)
        {
            if (isAlive)
            {
                hp -= damage;
                if (hp <= 0)
                {
                    Die();
                }
                else
                {
                    //animacion de recibir damage
                    //sfx de recibir damage
                }
            }
        }

        protected override void EntityReset()
        {
            hp = 100;
            basicAtk = 10;
            powerAtk = 25;
            isAlive = true;
        }
    }
}