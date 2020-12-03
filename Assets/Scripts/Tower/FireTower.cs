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

    private void Start()
    {
        //Sets the parameters of the available upgrades for this tower.
        Upgrades = new TowerUpgrade[]
        {
            new TowerUpgrade(Price/2,   //Price
                             1,         //Damage
                             25,         //Range
                             0,         //Attack cooldown
                             0,         //Duration
                             10,         //ProcChance
                             2,         //TickDamage
                             0),        //SlowFactor

            new TowerUpgrade(Price,   //Price
                             2,         //Damage
                             25,         //Range
                             0.1f,         //Attack cooldown
                             0,         //Duration
                             10,         //ProcChance
                             4,         //TickDamage
                             0),        //SlowFactor

            new TowerUpgrade(Price,   //Price
                             2,         //Damage
                             25,         //Range
                             0.1f,         //Attack cooldown
                             0,         //Duration
                             10,         //ProcChance
                             4,         //TickDamage
                             0),        //SlowFactor

            new TowerUpgrade(Price*2,   //Price
                             2,         //Damage
                             50,         //Range
                             0.2f,         //Attack cooldown
                             0,         //Duration
                             10,         //ProcChance
                             4,         //TickDamage
                             0),        //SlowFactor
        };
    }

    public override Debuff GetDebuff()
    {
        return new FireDebuff(TickDamage, TickTime, DebuffDuration, Target);
    }

    public override string SetTooltip()
    {
        return string.Format("<color=#ffa500ff>" + 
                             "{0}\n" +
                             "Damage Per Second: {1}" +
                             "</color>",
                             base.SetTooltip(),
                             tickDamage / tickTime);

    }

    public override void Upgrade()
    {
        this.TickDamage += NextUpgrade.TickDamage;
        base.Upgrade();
    }



}
