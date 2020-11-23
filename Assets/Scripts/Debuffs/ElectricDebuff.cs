using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricDebuff : Debuff
{
    public ElectricDebuff(float duration, Monster target) : base(target, duration)
    {

    }

    public override void Update()
    {
        if (target != null)
        {
            target.CurrentSpeed = 0;
        }
        base.Update();
    }

    public override void Remove()
    {
        target.CurrentSpeed = target.MaxSpeed;
        base.Remove();
    }

}
