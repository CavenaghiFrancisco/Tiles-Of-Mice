namespace TOM.Enemy
{
    public enum Flags
    {
        OnEveryEnemySpawned,        // Reach the max number of enemies
        OnArenaArrived,             // Reach the arena after spawning
        OnReachedTarget,            // Reach the target position
        OnBasicAttack,              // Has attacked
        OnPowerAttack,              // Has attacked
        OnGettingDamage,            // Is being damaged
        OnStunFinish,               // Finish stun time
        OnHurtFinish,               // Finish waiting time
        OnGettingFatalDamage,       // Is near to death...
        OnDie                       // Died
    }
}