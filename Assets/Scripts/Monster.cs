using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [SerializeField]
    private Stat health;

    public Stat Health
    {
        get
        {
            return health;
        }

        set
        {
            this.health = value;
        }
    }

    [SerializeField]
    private float currentSpeed;

    public float CurrentSpeed
    {
        get
        {
            return currentSpeed;
        }
        set
        {
            this.currentSpeed = value;
        }
    }
    [SerializeField]
    private int goldReward;

    public int GoldReward
    {
        get
        {
            return goldReward;
        }
        set
        {
            goldReward = value;
        }
    }

    //public Dictionary<Point, TileScript> Tiles { get; set; }

    //public Dictionary<Debuff, > newDebuff = new Dictionary<Debuff, bool>();


    private List<Debuff> newDebuffs = new List<Debuff>();
    private List<Debuff> debuffs = new List<Debuff>();
    private List<Debuff> expiredDebuffs = new List<Debuff>();

    
    private bool allowMovement;

    public bool AllowMovement
    {
        get
        {
            return allowMovement;
        }
        set
        {
            allowMovement = value;
        }
    }

    

    public float MaxSpeed { get; set; }

    private Stack<Node> path;

    public Point GridPosition { get; set; }

    private Vector3 destination;

    
    private Animator monsterAnimator;

    

    /// <summary>
    /// Moves the monster
    /// </summary>
    private void MoveMonster()
    {
        //Check if the monster is allowed to move
        if (allowMovement)
        {
            
            //RotateMonster();

            transform.position = Vector3.MoveTowards(transform.position, destination, currentSpeed * Time.deltaTime);

            if (transform.position == destination)
            {
                if (path != null && path.Count > 0)
                {
                    Animate(GridPosition, path.Peek().GridPosition);

                    GridPosition = path.Peek().GridPosition;
                    destination = path.Pop().WorldPosition;
                }
            }
        }
    }

    private void Animate(Point currentPos, Point nextPos)
    {
        if (currentPos.Y > nextPos.Y)
        {
            //moving down
            monsterAnimator.SetInteger("Horizontal", 0);
            monsterAnimator.SetInteger("Vertical", -1);
        }
        else if (currentPos.Y < nextPos.Y)
        {
            //moving up
            monsterAnimator.SetInteger("Horizontal", 0);
            monsterAnimator.SetInteger("Vertical", 1);
        }
        else if (currentPos.X > nextPos.X)
        {
            //moving left
            monsterAnimator.SetInteger("Horizontal", -1);
            monsterAnimator.SetInteger("Vertical", 0);
        }
        else if (currentPos.X < nextPos.X)
        {
            //moving right
            monsterAnimator.SetInteger("Horizontal", 1);
            monsterAnimator.SetInteger("Vertical", 0);
        }
    }



    /// <summary>
    /// Sets the path that the monster will follow.
    /// </summary>
    private void SetPath(Stack<Node> newPath)
    {
        if(newPath != null) // checks if the new path exists. 
        {
            this.path = newPath; // if the path exists, then the monster's path will be set to the new path.

            GridPosition = path.Peek().GridPosition; // sets the new grid position of the monster.

            Animate(GridPosition, path.Peek().GridPosition);

            destination = path.Pop().WorldPosition; // as the monsters reach their destination, update the destination to the next worldposition in the queue.
        }
    }

    /// <summary>
    /// Spawns the monsters. Uses scale to make the monster start from a smaller size then scale into a bigger size to give the effect of coming through a portal.
    /// </summary>
    public void Spawn(int health)
    {



        transform.position = LevelManager.Instance.StartPortal.transform.position; // sets the position where the monster will spawn.

        monsterAnimator = transform.GetChild(0).GetComponent<Animator>(); //sets the animator component of the monster.

        this.health.MaxVal += health;
        this.health.CurrentVal = this.health.MaxVal; //Sets the current health of the monster to the max value.

        AudioManager.Instance.PlaySFX("Spawn"); // plays the spawn sound effect.

        StartCoroutine(Scale(new Vector3(0.1f, 0.1f), new Vector3(1, 1), false));// starts the scaling function alongside the spawn function.

        
        SetPath(LevelManager.Instance.Path); // sets the path that the monster will follow.
    }

    /// <summary>
    /// Function used to change the size of the monster
    /// </summary>
    /// <param name="from">Initial Size</param>
    /// <param name="to">Target Size</param>
    /// <param name="remove">Check if the monster is spawning or despawning </param>
    /// <returns></returns>
    public IEnumerator Scale(Vector3 from, Vector3 to, bool remove)
    {
        float progress = 0;

        while (progress <= 1)
        {
            transform.localScale = Vector3.Lerp(from, to, progress);

            progress += Time.deltaTime;

            yield return null;
        }

        transform.localScale = to;

        allowMovement = true;

        if (remove == true)
        {
            Release();
        }
    }

    public void AddDebuff(Debuff debuff)
    {
        
        
        if(!debuffs.Exists(x => x.GetType() == debuff.GetType()) && !newDebuffs.Exists(x => x.GetType() == debuff.GetType())) // prevents same type of debuffs stacking
        {
            //Debuffs are first added to the list newDebuffs
            newDebuffs.Add(debuff);
        }

        
    }

    private void HandleDebuffs()
    {
        if (expiredDebuffs.Count > 0)
        {
            //Checks for each debuff that has expired
            foreach (Debuff debuff in expiredDebuffs)
            {
                //removes each expired debuff before handling them.
                debuffs.Remove(debuff);
            }

            expiredDebuffs.Clear();
        }

        //Checks if there are new debuffs to be placed on the monster
        if (newDebuffs.Count > 0)
        {
            debuffs.AddRange(newDebuffs);
            newDebuffs.Clear();
        }
           

        foreach (Debuff debuff in debuffs)
        {
            debuff.Update();
        }
    }

    public void RemoveDebuff(Debuff debuff)
    {
        expiredDebuffs.Add(debuff);
    }


    public void TakeDamage (float damage)
    {
        if (isActiveAndEnabled)
        {
            health.CurrentVal = Mathf.Clamp(health.CurrentVal - damage, 0, health.MaxVal) ;
            AudioManager.Instance.PlaySFX("EnemyDamaged");

            if (health.CurrentVal <= 0)
            {
                AudioManager.Instance.PlaySFX("EnemyDies");
                GameManager.Instance.Currency += this.goldReward;
                AudioManager.Instance.PlaySFX("GetMoney");
                
                Release();
            }
        }
        

    }

    private void Release()
    {
        expiredDebuffs.AddRange(debuffs); //remove all debuffs once monster is released.
        allowMovement = false;
        currentSpeed = MaxSpeed;
        GameManager.Instance.Pool.ReleaseObject(gameObject);
        GameManager.Instance.RemoveMonster(this); //removes the monster from the list of active monsters.
    }

    //When something enters the monsters hitbox.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Goal")
        {
            //Scale the monster down once it enters the goal
            StartCoroutine(Scale(new Vector3(1, 1), new Vector3(0.1f, 0.1f), true));
            GameManager.Instance.PlayerLives--;// take away one of the player lives upon a monster reaching the goal.
            AudioManager.Instance.PlaySFX("PlayerLivesLost");
        }

        if(collision.tag == "Tile")
        {
            //change the layer sorting order when the monster enters a new tile.
            transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = collision.GetComponent<TileScript>().GridPosition.Y;
        }
        
    }

    private void Awake()
    {
        MaxSpeed = currentSpeed;
        health.Initialize();
        
    }

    private void Update()
    {
        MoveMonster(); //calls the move monster function every tick.
        HandleDebuffs();
       
    }
}
