using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        [SerializeField] private AnimationClip attack1 = null;
        [SerializeField] private AnimationClip attack2 = null;

        private readonly int attack1AnimationHash = Animator.StringToHash("Br1e_Attack1");
        private readonly int attack2AnimationHash = Animator.StringToHash("Br1e_Attack2");

        private Controls controls;
        private Movement movement;
        private Animator animator;

        private bool isAttacking;
        private Vector3 originalPosition;

        Color defaultColor = default;

        public Action<int> OnLifeModified;

        public bool IsAttacking { get => isAttacking; }

        private void Awake()
        {
            originalPosition = transform.position;
            EntityReset();
            defaultColor = playerMat.color;
            controls = new Controls();
            movement = GetComponent<Movement>();

            animator = GetComponent<Animator>();

            OnDeath += EntityReset;

            controls.Attack.BasicAttack.performed += context =>
                {
                    if (!movement.IsDashing && attackArea.CanAttack)
                    {
                        int aux = UnityEngine.Random.Range(0, 10);
                        animator.Play(aux >= 5 ? attack1.name : attack2.name);
                        attackArea.GenerateAttackArea(aux >= 5 ? attack1.length : attack2.length, attackPosition.position);
                    }
                };
            //Intercambiar el 0.2f por la duracion de la animacion
            attackArea.OnEnemyHit += Attack;
            attackArea.OnAttack += SetAttackState;
        }
        private void OnDestroy()
        {
            playerMat.color = defaultColor;
            attackArea.OnEnemyHit -= Attack;
            attackArea.OnAttack -= SetAttackState;
            OnDeath -= EntityReset;
        }

        public override void Attack(Entity otherEntity)
        {
            if (isAlive)
            {
                otherEntity.GetDamage(this.basicAtk);
            }
        }

        public void SetAttackState(bool isAttacking)
        {
            this.isAttacking = isAttacking;
            movement.canDash = !isAttacking;
        }

        public override void Die()
        {
            if (isAlive)
            {
                hp = 0;
                OnLifeModified?.Invoke(hp);
                //animacion de muerte
                //sfx de muerte
                Debug.Log("Br1e ha muerto");
                isAlive = false;
                gameObject.SetActive(false); //Hacer esto una vez que se ejecuta la animacion de muerte
                OnDeath?.Invoke();
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
                OnLifeModified?.Invoke(hp);
            }
        }

        protected override void EntityReset()
        {
            hp = 10000;
            maxHP = 10000;
            basicAtk = 10;
            powerAtk = 25;
            hurtTime = 1f;
            isAlive = true;
            transform.position = originalPosition;
            gameObject.SetActive(true);
            OnLifeModified?.Invoke(hp);
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