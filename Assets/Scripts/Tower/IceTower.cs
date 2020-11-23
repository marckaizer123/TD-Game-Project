using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceTower : Tower
{
    [SerializeField]
    private float slowFactor;


    public override Debuff GetDebuff()
    {
        return new FrostDebuff(slowFactor, DebuffDuration, Target);
    }
}
