using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TOM.ATTACK
{
    public enum AttackType
    {
        Basic,
        Power
    }
}

namespace TOM
{
    public class Player : Entity
    {
        [SerializeField] private Material playerMat = null;
        [SerializeField] private Transform attackPosition = null;
        [SerializeField] private AttackArea attackArea = null;

        private Controls controls;
        private Movement movement;

        Color defaultColor = default;
        private void Awake()
        {
            EntityReset();
            defaultColor = playerMat.color;
            controls = new Controls();
            movement = GetComponent<Movement>();

            controls.Attack.BasicAttack.performed += context =>
            {
                if (!movement.IsDashing)
                {
                    attackArea.GenerateAttackArea(0.2f, attackPosition.position);
                }
            };
            //Intercambiar el 0.2f por la duracion de la animacion
            attackArea.OnEnemyHit += Attack;
        }
        private void OnDestroy()
        {
            playerMat.color = defaultColor;
            attackArea.OnEnemyHit -= Attack;
        }

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
                isAlive = false;
                OnDeath?.Invoke();
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
                    GetHurt(hurtTime);
                    //animacion de recibir damage
                    //sfx de recibir damage
                }
            }
        }

        protected override void EntityReset()
        {
            hp = 10000;
            basicAtk = 10;
            powerAtk = 25;
            hurtTime = 1f;
            isAlive = true;
        }

        private void GetHurt(float hurtTime)
        {
            StartCoroutine(RecieveDamage(hurtTime));
        }

        IEnumerator RecieveDamage(float hurtTime)
        {
            float t = 0;
            playerMat.color = Color.red;
            while (t < hurtTime)
            {
                t += Time.deltaTime;
                yield return null;
            }
            playerMat.color = defaultColor;
        }

        private void OnEnable()
        {
            controls.Attack.Enable();
        }

        private void OnDisable()
        {
            controls.Attack.Disable();
        }
    }
}