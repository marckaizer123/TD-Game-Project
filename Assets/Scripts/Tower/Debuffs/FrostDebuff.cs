using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrostDebuff : Debuff
{

    private float slowFactor;

    private bool isApplied;

    public FrostDebuff(float slowFactor, float duration, Monster target) : base(target, duration)
    {
        this.slowFactor = slowFactor;
    }

    public override void Update()
    {
        if (target != null)
        {
            if (!isApplied)
            {
                isApplied = true;

                target.CurrentSpeed = target.MaxSpeed * slowFactor / 100;
            }
        }
        base.Update();
    }

    public override void Remove()
    {
        target.CurrentSpeed = target.MaxSpeed;     
        base.Remove();
    }
}
