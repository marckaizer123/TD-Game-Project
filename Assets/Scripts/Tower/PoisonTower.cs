using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonTower : Tower
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

    private void Start()
    {
        //Sets the parameters of the available upgrades for this tower.
        Upgrades = new TowerUpgrade[]
        {
            new TowerUpgrade(Price/2,   //Price
                             1,         //Damage
                             25,         //Range
                             0,         //Attack cooldown
                             1,         //Duration
                             0,         //ProcChance
                             0.25f,         //TickDamage
                             0),        //SlowFactor

            new TowerUpgrade(Price,   //Price
                             1,         //Damage
                             25,         //Range
                             0,         //Attack cooldown
                             1,         //Duration
                             0,         //ProcChance
                             0.25f,         //TickDamage
                             0),        //SlowFactor

            new TowerUpgrade(Price,   //Price
                             1,         //Damage
                             25,         //Range
                             0,         //Attack cooldown
                             1,         //Duration
                             0,         //ProcChance
                             0.25f,         //TickDamage
                             0),        //SlowFactor

            new TowerUpgrade(Price*2,   //Price
                             1,         //Damage
                             25,         //Range
                             0,         //Attack cooldown
                             2,         //Duration
                             0,         //ProcChance
                             0.5f,         //TickDamage
                             0),        //SlowFactor
        };
    }

    public override Debuff GetDebuff()
    {
        return new PoisonDebuff(TickDamage, TickTime, DebuffDuration, Target);
    }

    public override string SetTooltip()
    {
        return string.Format("<color=#00ff00ff>" +
                             "{0}\n" +
                             "Damage Per Second: {1}" +
                             "</color>",
                             base.SetTooltip(),
                             this.tickDamage/this.tickTime);

    }

    public override void Upgrade()
    {
        this.TickDamage += NextUpgrade.TickDamage;
        base.Upgrade();
    }


}
