public abstract class Enemy : Entity, ICombatable, IHittable
{
    protected BehaviorType behavior;

    public virtual void GetHit(int amount)
    {
        hitPoints -= amount/* *100/def */;
    }
    public virtual void Hit(Entity entity)
    {
        //Do something
    }
    public virtual void PowerHit(Entity entity)
    {
        //Do something
    }
}
