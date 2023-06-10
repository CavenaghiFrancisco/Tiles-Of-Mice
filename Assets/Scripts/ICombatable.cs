using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICombatable
{
    void Hit(Entity entity);
    void PowerHit(Entity entity);
}
