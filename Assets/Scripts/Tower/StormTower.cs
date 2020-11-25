using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StormTower : Tower
{
    

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

    private void Start()
    {
        Upgrades = new TowerUpgrade[]
        {
            new TowerUpgrade(2, 2, 2, .5f, 5, 1),
            new TowerUpgrade(2, 2, 2, .5f, 5, 1)
        };
    }
}
