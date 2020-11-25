using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerUpgrade
{

    public int Price { get; private set; }

    public int Damage { get; private set; }

    public float DebufDuration { get; private set; }

    public float ProcChance { get; private set; }

    public float SlowFactor { get; private set; }

    public float Ticktime { get; private set; }

    public float Range { get; private set; }

    public TowerUpgrade(int price, int damage, float range, float debuffDuration, float procChance, float slowfactor)
    {
        this.Damage = damage;
        this.Range = range;
        this.DebufDuration = debuffDuration;
        this.ProcChance = procChance;
        this.SlowFactor = slowfactor;
        this.Price = price;
    }


}
