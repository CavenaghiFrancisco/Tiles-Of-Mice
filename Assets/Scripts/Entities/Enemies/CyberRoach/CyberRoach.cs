using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TOM.Enemy
{
    public class CyberRoach : Enemy
    {
        [SerializeField] private int testingGetDamage = 20;//Esto hay que sacarlo, es solo con motivos de testeo;
        Color defaultColor = default;
        Material material = null;

        public void Initialize(int HP, int BasicAtk, int PowerAtk, int powerAttackChance,float basicAtkCD,  float powerAtkCD, float attackRadius, float stunTimeInms, int Speed, float hurtTime)
        {
            hp = HP;
            type = EnemyType.Normal;
            basicAtk = BasicAtk;
            powerAtk = PowerAtk;
            basicHitCD = basicAtkCD;
            powerHitCD = powerAtkCD;
            powerHitChance = powerAttackChance;
            this.stunTimeInms = stunTimeInms;
            this.attackRadius = attackRadius;
            moveSpeed = Speed;
            this.hurtTime = hurtTime;
            isAlive = true;
        }
        public void Initialize(EnemyParameters param) => Initialize
        (
            param.healthPoints,
            param.basicAttack, param.powerAttack,
            param.powerAttackChance,
            param.basicAttackCoolDown, param.powerAttackCoolDown,
            param.attackRadius,
            param.stunTimeInms,
            param.movementSpeed,
            param.hurtTime
        );

        private void Awake()
        {
            EntityReset();
            material = GetComponent<MeshRenderer>().material;
            defaultColor = material.color;
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
                //animacion de muerte
                //sfx de muerte
                Debug.Log("Un CyberRoach ha muerto!");
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
                    //Animación de recibir damage
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
            //Animación de ataque básico
            //Sfx del ataque básico
            return basicAtk;
        }
        private int PowerHit()
        {
            Debug.Log(name + " hace un Power Hit");
            //Animación de ataque pesado
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
            material.color = Color.green;
            while (t < hurtTime)
            {
                t += Time.deltaTime;
                yield return null;
            }
            material.color = defaultColor;
        }
        //private void OnCollisionEnter(Collision collision)
        //{
        //    if(collision.collider.tag=="Player")
        //    {
        //        GetDamage(testingGetDamage);
        //    }
        //}
    }

}