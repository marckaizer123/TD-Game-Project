using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Tower parent;

    private Monster target;


    public void Initialize(Tower parent)
    {
        this.parent = parent;
        this.target = parent.Target;
    }


    private void FollowTarget()
    {
        //projectile only moves when there is a target and the target is active.
        if (target != null && target.isActiveAndEnabled)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * parent.ProjectileSpeed);

            Vector3 faceDirection = target.transform.position - transform.position;
            float rotationAngle = Mathf.Atan2(faceDirection.y, faceDirection.x) * 180 / Mathf.PI;
            transform.rotation = Quaternion.AngleAxis(rotationAngle, Vector3.forward);
        }

        else if (!target.isActiveAndEnabled)
        {
            //Release the projectile when its target becomes inactive.
            GameManager.Instance.Pool.ReleaseObject(gameObject); 
        }
    }

    /// <summary>
    /// The script that runs upon the projectile colliding with the monster.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Monster")
        {
            
            if(target.gameObject == collision.gameObject)
            {
                target.TakeDamage(parent.Damage);

                ApplyDebuff();
                
            }

            GameManager.Instance.Pool.ReleaseObject(gameObject);
            
        }
    }

    private void ApplyDebuff()
    {
        float roll = Random.Range(0, 100);

        if (roll < parent.ProcChance)
        {
            target.AddDebuff((parent.GetDebuff()));
        }

        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        FollowTarget();
    }
}
