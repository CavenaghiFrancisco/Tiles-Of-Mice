using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CyberRoach : Enemy
{
    public override void Attack(Entity otherEntity)
    {
        otherEntity.GetDamage(this.atk);
        //animacion de ataque
        //sonido de ataque
    }

    public override void Die()
    {
        hp = 0;
        //animacion de muerte
        //sfx de muerte
        Debug.Log("Br1e se esta muerto");
        this.enabled = false;
    }

    public override void GetDamage(int damage)
    {
        hp -= damage;
        if (hp <= 0) Die();
    }

    protected override void EntityReset()
    {
        throw new System.NotImplementedException();
    }
}
