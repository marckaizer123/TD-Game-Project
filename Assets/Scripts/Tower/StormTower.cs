using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StormTower : Tower
{
    

    public override Debuff GetDebuff()
    {
        return new ElectricDebuff(DebuffDuration, Target);
    }
}
