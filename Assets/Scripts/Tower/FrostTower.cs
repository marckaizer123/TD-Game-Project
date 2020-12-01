using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrostTower : Tower
{
    [SerializeField]
    private float slowFactor;

    public float SlowFactor
    {
        get
        {
            return slowFactor;
        }
        set
        {
            slowFactor = value;
        }
    }

    private void Start()
    {
        //Sets the parameters of the available upgrades for this tower.
        Upgrades = new TowerUpgrade[]
        {
            new TowerUpgrade(Price/2,   //Price
                             2.5f,         //Damage
                             25,         //Range
                             0,         //Attack cooldown
                             0.5f,         //Duration
                             0,         //ProcChance
                             0,         //TickDamage
                             10),        //SlowFactor

            new TowerUpgrade(Price,   //Price
                             2.5f,         //Damage
                             25,         //Range
                             0,         //Attack cooldown
                             0.5f,         //Duration
                             0,         //ProcChance
                             0,         //TickDamage
                             10),        //SlowFactor

            new TowerUpgrade(Price,     //Price
                             2.5f,       //Damage
                             25,         //Range
                             0,         //Attack cooldown
                             0.5f,         //Duration
                             0,         //ProcChance
                             0,         //TickDamage
                             10),        //SlowFactor

            new TowerUpgrade(Price*2,   //Price
                             5,         //Damage
                             75,         //Range
                             0,         //Attack cooldown
                             0.5f,         //Duration
                             0,         //ProcChance
                             0,         //TickDamage
                             10),        //SlowFactor
        };
    }

    public override Debuff GetDebuff()
    {
        return new FrostDebuff(slowFactor, DebuffDuration, Target);
    }

    public override string SetTooltip()
    {
        return string.Format("<color=#00ffffff> " +
                             "{0}\n" +
                             "Slow: {1}%" +
                             "</color>",
                             base.SetTooltip(),
                             slowFactor);

    }

    public override void Upgrade()
    {
        this.SlowFactor += NextUpgrade.SlowFactor;
        base.Upgrade();
    }

    
}
