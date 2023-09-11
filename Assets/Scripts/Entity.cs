using UnityEngine;
using IA.FSM;

public abstract class Entity : MonoBehaviour
{
    protected int hp;
    protected int atk;
    protected float hitCD;
    protected FSM fsm;
    protected bool isAlive;
    public abstract void GetDamage(int damage);
    public abstract void Die();
    public abstract void Attack(Entity otherEntity);
    protected abstract void EntityReset();
}
