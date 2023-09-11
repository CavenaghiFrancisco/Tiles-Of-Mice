using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    private void Awake() => EntityReset();

    public override void Attack(Entity otherEntity)
    {
        if (isAlive)
        {
            otherEntity.GetDamage(this.atk);
        }
    }

    public override void Die()
    {
        if (isAlive)
        {
            hp = 0;
            //animacion de muerte
            //sfx de muerte
            Debug.Log("Br1e se esta muerto");
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

    private void OnTriggerEnter(Collider other)
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

    protected override void EntityReset()
    {
        hp = 100;
        atk = 10;
        isAlive = true;
    }
}