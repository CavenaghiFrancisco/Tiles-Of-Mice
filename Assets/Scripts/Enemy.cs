public abstract class Enemy : Entity, ICombatable, IHittable
{
    protected int hitPoints;
    protected int attack;
    protected int def;

    protected BehaviorType behavior;
    public int HP { get => hitPoints; }
    public int ATK { get => attack; }
    public int DEF { get => def; }

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
