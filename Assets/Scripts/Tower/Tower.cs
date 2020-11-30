using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// This will be inherited by the more specialized tower types.
/// </summary>
public abstract class Tower : MonoBehaviour
{
    [SerializeField]
    private string towerType;

    public string TowerType
    {
        get
        {
            return towerType;
        }
    }

    private int towerLevel;

    public int TowerLevel
    {
        get
        {
            return towerLevel;
        }

        protected set
        {
            towerLevel = value;
        }
    }


    public int Price { get; set; }

    [SerializeField]
    private float range;

    public float Range
    {
        get
        {
            return range;
        }
        set
        {
            range = value;
        }
    }

    private Vector3 towerRange;

    public Vector3 TowerRange
    {
        get
        {
            return towerRange;
        }
        set
        {
            towerRange = value;
        }
    }

    private SpriteRenderer rangeRenderer;

    [SerializeField]
    private float damage;

    public float Damage
    {
        get
        {
            return damage;
        }
    }

    //Time needed before a tower is allowed to attack again.
    [SerializeField]
    private float attackCooldown;

    public float AttackCooldown
    {
        get
        {
            return attackCooldown;
        }

        set
        {
            attackCooldown = value;
        }
    }

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
    private string debuffType;

    public string DebuffType
    {
        get
        {
            return debuffType;
        }
        set
        {
            debuffType = value;
        }
    }

    [SerializeField]
    private float procChance;

    public float ProcChance
    {
        get
        {
            return procChance;
        }

        set
        {
            this.procChance = value;
        }
    }

    [SerializeField]
    private float debuffDuration;

    public float DebuffDuration
    {
        get
        {
            return debuffDuration;
        }

        set
        {
            this.debuffDuration = value;
        }
    }

    //Reference to the array containing all the upgrades of a tower.
    public TowerUpgrade[] Upgrades { get; protected set; }


    //Reference to the next upgrade on the array.
    public TowerUpgrade NextUpgrade
    {
        get
        {
            if (Upgrades.Length >= towerLevel)
            {
                return Upgrades[towerLevel - 1];
            }

            return null;
        }
    }

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

    private void Awake()
    {
        TowerLevel = 1;
        rangeRenderer = GetComponent<SpriteRenderer>();
        towerRange = new Vector3(range, range, 0);
        transform.localScale = towerRange;
    }


    /// <summary>
    /// Function to show/hide the range of a tower when selected/unselected.
    /// </summary>
    public void Select()
    {
        //shows and hides the range when tower is selected.
        rangeRenderer.enabled = !rangeRenderer.enabled;
    }

    /// <summary>
    /// Gets the stats of the tower and displays it on the stats panel.
    /// </summary>
    public void GetStats()
    {

        string tooltip = this.SetTooltip();
        GameManager.Instance.SetToolTipText(tooltip);
        GameManager.Instance.ShowStats();
    }

    /// <summary>
    /// Sets the text that will be shown in the stat panel.
    /// </summary>
    /// <returns></returns>
    public virtual string SetTooltip()
    {


        return string.Format("<size=65><b>{0}</b></size>\n\n" +
                             "Level: {1}\n" +
                             "Damage: {2}\n" +
                             "Attack Speed: Every {3} sec\n" +
                             "Range: {4}\n" +
                             "Debuff: {5}\n" +
                             "Chance: {6}%\n" +
                             "Duration: {7}",
                             towerType,
                             towerLevel,
                             damage,
                             attackCooldown,
                             range,
                             debuffType,
                             procChance,
                             debuffDuration);
    }

    public virtual void Upgrade()
    {
        //Reduce currency according to upgrade price.
        GameManager.Instance.Currency -= NextUpgrade.Price;
        Price += NextUpgrade.Price;
        this.damage += NextUpgrade.Damage;
        this.procChance += NextUpgrade.ProcChance;
        this.range += NextUpgrade.Range;
        this.AttackCooldown -= NextUpgrade.AttackCooldown;
        this.debuffDuration += NextUpgrade.DebuffDuration;
        TowerLevel++;

        towerRange = new Vector3(range, range, 0);
        transform.localScale = towerRange;
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

    public abstract Debuff GetDebuff();


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

    /// <summary>
    /// function that tells the tower what target to attack, when to attack, and to initialize the attack.
    /// </summary>
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

        //Check if tower does not have a target and there is a monster in range
        if (target == null && monsterTargets.Count > 0 && monsterTargets.Peek().AllowMovement)
        {
            //removes the first monster in the queue and sets them as the target.
            target = monsterTargets.Dequeue();
        }

        //if the tower has a target and the target is active and enabled and target is alive


        if (target != null && (target.isActiveAndEnabled == false || target.Health.CurrentVal <= 0))
        {
            target = null;
        }

        if (target != null && target.isActiveAndEnabled == true && target.Health.CurrentVal > 0)
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

    /// <summary>
    /// Function that handles the spawning of projectile.
    /// </summary>
    private void Shoot()
    {
        //AudioManager.Instance.PlaySFX("Shoot");
        // gets a projectile from the pool.
        Projectile projectile = (Projectile)GameManager.Instance.Pool.GetObject(projectileType).GetComponent<Projectile>();

        // sets the initial position of the projectile on the position of the tower.
        projectile.transform.position = transform.position;

        //pass the tower to the initialize function in the projectile class.
        projectile.Initialize(this);
    }


    

    

    // Update is called once per frame
    void Update()
    {
        Attack();
    }
}
