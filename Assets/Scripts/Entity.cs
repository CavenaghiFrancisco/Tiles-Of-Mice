using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    protected int hitPoints;
    protected int attack;
    protected int def;

    public int HP { get => hitPoints; }
    public int ATK { get => attack; }
    public int DEF { get => def; }

}
