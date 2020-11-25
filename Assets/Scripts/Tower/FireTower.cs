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

    public override string SetTooltip()
    {
        return string.Format("<color=#ffa500ff> " + 
                             "{0}\n" +
                             "Damage Per Second: {1}" +
                             "</color>",
                             base.SetTooltip(),
                             tickDamage / tickTime);

    }

    private void Start()
    {
        Upgrades = new TowerUpgrade[]
        {
            new TowerUpgrade(2, 2, 2, .5f, 5, 1),
            new TowerUpgrade(2, 2, 2, .5f, 5, 1)
        };
    }

}
