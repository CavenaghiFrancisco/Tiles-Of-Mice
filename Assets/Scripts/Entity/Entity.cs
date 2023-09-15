using System;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    [SerializeField] protected float life;
    [SerializeField] protected float maxLife;

    public Action<float> OnMaxLifeModified;
    public Action<float> OnLifeModified;

    public float GetLife()
    {
        return life;
    }

    public void SetLife(float newLife)
    {
        life = newLife > maxLife ? maxLife : newLife;
        OnLifeModified?.Invoke(life);
    }

    public float GetMaxLife()
    {
        return maxLife;
    }

    public void SetMaxLife(float newMaxLife)
    {
        maxLife = newMaxLife;
        OnMaxLifeModified?.Invoke(maxLife);
    }
}
