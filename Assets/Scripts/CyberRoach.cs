public class CyberRoach : Enemy
{
    public CyberRoach()
    {
        behavior = BehaviorType.Follower;

        hitPoints = 10;
        attack = 2;
        def = 2;

    }

    public override void GetHit(int amount)
    {
        hitPoints -= amount/* *100/def */;
    }
    public override void Hit(Entity entity)
    {
        //Do something
    }
    public override void PowerHit(Entity entity)
    {
        //Do something
    }
}
