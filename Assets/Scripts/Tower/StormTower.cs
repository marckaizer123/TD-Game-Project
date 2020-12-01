using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StormTower : Tower
{
    private void Start()
    {
        //Sets the parameters of the available upgrades for this tower.
        Upgrades = new TowerUpgrade[]
        {
            new TowerUpgrade(Price/2,   //Price
                             5,         //Damage
                             25,         //Range
                             .1f,         //Attack cooldown
                             .1f,         //Duration
                             5,         //ProcChance
                             0,         //TickDamage
                             0),        //SlowFactor

            new TowerUpgrade(Price,   //Price
                             5,         //Damage
                             25,         //Range
                             .1f,         //Attack cooldown
                             .1f,         //Duration
                             5,         //ProcChance
                             0,         //TickDamage
                             0),        //SlowFactor

            new TowerUpgrade(Price,   //Price
                             5,         //Damage
                             50,         //Range
                             .1f,         //Attack cooldown
                             .1f,         //Duration
                             5,         //ProcChance
                             0,         //TickDamage
                             0),        //SlowFactor

            new TowerUpgrade(Price*2,   //Price
                             10,         //Damage
                             50,         //Range
                             .2f,         //Attack cooldown
                             .25f,         //Duration
                             10,         //ProcChance
                             0,         //TickDamage
                             0),        //SlowFactor

        };
    }

    public override Debuff GetDebuff()
    {
        return new ElectricDebuff(DebuffDuration, Target);
    }
    public override string SetTooltip()
    {
        return string.Format("<color=#add8e6ff>" +
                             "{0}" +
                             "</color>",
                             base.SetTooltip());

    }

    
}
