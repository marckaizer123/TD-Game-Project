using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower: MonoBehaviour
{
    
    private SpriteRenderer mySpriteRenderer;

    private Monster target;
    public Monster Target
    {
        get
        {
            return target;
        }
    }
    private Queue<Monster> monsterTargets = new Queue<Monster>();

    //Bool that determines whether or not the tower is allowed to attack
    private bool canAttack = true;

    //Time between attacks
    private float attackTimer;

    //Time needed before a tower is allowed to attack again.
    [SerializeField]
    private float attackCooldown;

    //The projectile that will be shot by the tower.
    [SerializeField]
    private string projectileType;

    //The speed of the projectile shot by the tower.
    [SerializeField]
    private float projectileSpeed;

    public float ProjectileSpeed
    {
        get
        {
            return projectileSpeed;
        }
    }

    [SerializeField]
    private float damage;

    public float Damage
    {
        get
        {
            return damage;
        }
    }






    /// <summary>
    /// Function to show/hide the range of a tower when selected/unselected.
    /// </summary>
    public void Select()
    {

        //shows and hides the range when tower is selected.
        mySpriteRenderer.enabled = !mySpriteRenderer.enabled;
    }

    /// <summary>
    /// Script that runs upon an enemy entering the range of the tower.
    /// </summary>
    /// <param name="collision"></param>
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Monster")
        {
            //puts all monsters that enter the range into a queue.
            monsterTargets.Enqueue(collision.GetComponent<Monster>());
        }
    }


    /// <summary>
    /// Script that runs upon an enemy leaving the range of the tower.
    /// </summary>
    /// <param name="collision"></param>
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Monster")
        {
            //sets the tower's target to null once their target leaves their range.
            target = null;
        }
    }

    private void Attack()
    {
        if (!canAttack)
        {
            //attack timer increases whenever the tower cannot attack.
            attackTimer += Time.deltaTime;

            if (attackTimer >= attackCooldown)
            {
                //allows the tower to attack again after the attack cooldown has been reached.
                canAttack = true;

                //resets the attack timer.
                attackTimer = 0;
            }
        }

        //Check if tower is does not have a target and there is a monster in range
        if (target == null && monsterTargets.Count>0)
        {
            //removes the first monster in the queue and sets them as the target.
            target = monsterTargets.Dequeue();
        }

        if (target != null && target.isActiveAndEnabled)
        {
            if (canAttack)
            {
                // calls the shoot function if the tower can attack
                Shoot();

                //prevents the tower from attacking immediately after shooting.
                canAttack = false;
            }
            
        }
    }

    private void Shoot()
    {
        // gets a projectile from the pool.
        Projectile projectile = (Projectile)GameManager.Instance.Pool.GetObject(projectileType).GetComponent<Projectile>();

        // sets the position of the projectile on the position of the tower.
        projectile.transform.position = transform.position;

        //pass the tower to the initialize function in the projectile class.
        projectile.Initialize(this);
    }


    // Start is called before the first frame update
    void Start()
    {
        mySpriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

        Attack();
    }
}
