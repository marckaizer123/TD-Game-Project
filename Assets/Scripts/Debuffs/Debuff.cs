using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Debuff
{
    protected Monster target;

    public Debuff(Monster target, float duration)
    {
        this.target = target;
        this.duration = duration;

    }

    private float duration;

    private float timeElapsed;

    public virtual void Update()
    {
        timeElapsed += Time.deltaTime;

        if(target!=null && timeElapsed>= duration)
        {
            Remove();
        }
    }

    public virtual void Remove()
    {
        if (target != null)
        {
            target.RemoveDebuff(this);
        }
    }
}
