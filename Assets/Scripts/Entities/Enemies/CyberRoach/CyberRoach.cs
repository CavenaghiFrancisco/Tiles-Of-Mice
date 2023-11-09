using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TOM.Enemy
{
    public class CyberRoach : Enemy
    {
        [SerializeField] private int testingGetDamage = 20;//Esto hay que sacarlo, es solo con motivos de testeo;

        private EnemyGrowParameters selfGrowParameters = null;
        private EnemyBasicParameters selfBasicParameters = null;

        public void Initialize(EnemyBasicParameters basicParameters, EnemyGrowParameters growParameters)
        {
            hp = basicParameters.healthPoints;
            type = EnemyType.Normal;
            basicAtk = basicParameters.basicAttack;
            powerAtk = basicParameters.powerAttack;
            basicHitCD = basicParameters.basicAttackCoolDown;
            powerHitCD = basicParameters.powerAttackCoolDown;
            powerHitChance = basicParameters.powerAttackChance;
            this.stunTimeInms = stunTimeInms;
            this.attackRadius = attackRadius;
            moveSpeed = basicParameters.movementSpeed;
            this.hurtTime = hurtTime;
            isAlive = false;
            animator = GetComponent<Animator>();

            selfBasicParameters = basicParameters;
            selfGrowParameters = growParameters;

        }

        private void Awake()
        {
            EntityReset();
        }
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
                isAlive = false;
                animator.SetTrigger("Death");
                //Debug.Log("Un CyberRoach ha muerto!");
                OnDeath?.Invoke();
                gameObject.SetActive(false); //Hacer esto al final de la animacion de muerte
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
                    //Animaci�n de recibir damage
                    //sfx de recibir damage
                }
                else
                {
                    GetHurt(hurtTime);
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
            Debug.Log(name + " hace un Basic Hit");
            animator.SetTrigger("Attack");
            //Sfx del ataque b�sico
            return basicAtk;
        }
        private int PowerHit()
        {
            Debug.Log(name + " hace un Power Hit");
            //Animaci�n de ataque pesado
            //Sfx del ataque pesado
            return powerAtk;//TBD
        }

        private int AttackDecider()
        {
            if (powerHitChance == 0) return BasicHit();
            return Random.Range(1, 101) < powerHitChance ? PowerHit() : BasicHit();
        }

        private void GetHurt(float hurtTime)
        {
            StartCoroutine(RecieveDamage(hurtTime));
        }

        IEnumerator RecieveDamage(float hurtTime)
        {
            float t = 0;
            while (t < hurtTime)
            {
                t += Time.deltaTime;
                yield return null;
            }
        }

        public override void Grow(int waveLevel)
        {   //Agrega los valores necesarios a la vida, ataques y movimiento dependiendo de la wave
            isAlive = true;

            hp = selfBasicParameters.healthPoints + (int)(waveLevel * selfGrowParameters.growingHealthPoints);
            moveSpeed = selfBasicParameters.movementSpeed + (int)(waveLevel * selfGrowParameters.growingMovementSpeed);
            basicAtk = selfBasicParameters.basicAttack + (int)(waveLevel * selfGrowParameters.growingBasicAttack);
            powerAtk = selfBasicParameters.powerAttack + (int)(waveLevel * selfGrowParameters.growingPowerAttack);
        }
    }

}