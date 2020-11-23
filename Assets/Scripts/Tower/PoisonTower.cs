using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonTower : Tower
{
    public override Debuff GetDebuff()
    {
        return new PoisonDebuff(Target);
    }
}
