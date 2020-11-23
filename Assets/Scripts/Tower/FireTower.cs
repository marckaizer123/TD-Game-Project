using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTower : Tower
{
    [SerializeField]
    private float tickTime;

    public float TickTime
    {
        get { return tickTime; }
    }

    [SerializeField]
    private float tickDamage;

    public float TickDamage
    {
        get { return tickDamage; }
        set { tickDamage = value; }
    }


    public override Debuff GetDebuff()
    {
        return new FireDebuff(TickDamage, TickTime, DebuffDuration, Target);
    }
}
