using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TOM
{
    public class Player : Entity
    {
        [SerializeField] private Material playerMat = null;
        
        Color defaultColor = default;
        private void Awake()
        { 
            EntityReset();
            defaultColor = playerMat.color;
        }
        private void OnDestroy()
        {
            playerMat.color = defaultColor;
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
            hp = 100;
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
    }
}