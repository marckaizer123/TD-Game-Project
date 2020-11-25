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

    private void Start()
    {
        Upgrades = new TowerUpgrade[]
        {
            new TowerUpgrade(2, 2, 2, .5f, 5, 1),
            new TowerUpgrade(2, 2, 2, .5f, 5, 1)
        };
    }
}
