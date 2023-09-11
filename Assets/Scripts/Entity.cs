using UnityEngine;
using IA.FSM;

public abstract class Entity : MonoBehaviour
{
    public int HP { private set;  get; }
    public int Atk { private set;  get; }

    private FSM fsm;
    public abstract void GetDamage(int damage);
    public abstract void Die();
    public abstract void Attack();
}
