using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TOM.Enemy
{
    public class CyberRoach : Enemy
    {
        public void Initialize(int HP, int BasicAtk, float BasicAtkCD, int PowerAtk, float PowerAtkCD, int Speed)
        {
            hp = HP;
            type = EnemyType.Normal;
            basicAtk = BasicAtk;
            powerAtk = PowerAtk;
            basicHitCD = BasicAtkCD;
            powerHitCD = PowerAtkCD;
            moveSpeed = Speed;
            isAlive = true;
        }
        public void Initialize(EnemyParameters param) => Initialize
        (
            param.healthPoints,
            param.basicAttack, param.basicAttackCoolDown,
            param.powerAttack, param.powerAttackCoolDown,
            param.movementSpeed
        );
        
        private void Awake() => EntityReset();
        public override void Attack(Entity otherEntity)
        {
            if (isAlive)
            {
                otherEntity.GetDamage(AttackDecider());
            }
        }

        public override void Die()
        {
            if (isAlive)
            {
                hp = 0;
                //animacion de muerte
                //sfx de muerte
                Debug.Log("Un CyberRoach ha muerto!");
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
        }

        private int BasicHit()
        {
            Debug.Log("Doing BasicHit");
            return basicAtk;
        }
        private int PowerHit()
        {
            Debug.Log("Doing PowerHit");
            return powerAtk;//TBD
        }

        private int AttackDecider()
        {
            return Random.Range(1, 101) < powerHitChance ? PowerHit() : BasicHit();
        }
    }

}