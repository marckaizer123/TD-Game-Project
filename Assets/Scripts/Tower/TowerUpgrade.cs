using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerUpgrade
{

    public int Price { get; private set; }

    public float Damage { get; private set; }

    public float AttackCooldown { get; private set; }

    public float DebuffDuration { get; private set; }

    public float ProcChance { get; private set; }

    public float SlowFactor { get; private set; }

    public float TickDamage { get; private set; }

    public float Range { get; private set; }

    public TowerUpgrade(int price, float damage,  float range, float attackCooldown, float debuffDuration, float procChance, float tickDamage, float slowfactor)
    {
        this.Damage = damage;
        this.Range = range;
        this.AttackCooldown = attackCooldown;
        this.DebuffDuration = debuffDuration;
        this.ProcChance = procChance;
        this.SlowFactor = slowfactor;
        this.Price = price;
    }


}
