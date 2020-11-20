using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{

    private bool allowMovement;

    [SerializeField]
    private float speed = 0;

    [SerializeField]
    private Stat health;



    private Stack<Node> path;

    public Point GridPosition { get; set; }

    private Vector3 destination;

    

    /// <summary>
    /// Moves the monster
    /// </summary>
    private void MoveMonster()
    {

        if (allowMovement)
        {
            RotateMonster();

            transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);

            if (transform.position == destination)
            {
                if (path != null && path.Count > 0)
                {
                    GridPosition = path.Peek().GridPosition;
                    destination = path.Pop().WorldPosition;
                }
            }
        }
    }

    /// <summary>
    /// Rotates the monster based on the direction it will move into.
    /// </summary>
    private void RotateMonster()
    {
        Vector3 faceDirection = destination - transform.position;

        float rotationAngle = Mathf.Atan2(faceDirection.y, faceDirection.x) * 180 / Mathf.PI - 90;


        //rotates only the monster sprite of the monster game object.
        transform.GetChild(0).transform.rotation = Quaternion.AngleAxis(rotationAngle, Vector3.forward);

    }

    /// <summary>
    /// Sets the path that the monster will follow.
    /// </summary>
    private void SetPath(Stack<Node> newPath)
    {
        if(newPath != null) // checks if the new path exists. 
        {
            this.path = newPath; // if the path exists, then the monster's path will be set to the new path.
            GridPosition = path.Peek().GridPosition; 
            destination = path.Pop().WorldPosition; // as the monsters reach their destination, update the destination to the next worldposition in the queue.
        }
    }

    //Spawns the monsters. Uses scale to make the monster start from a smaller size then scale into a bigger size to give the effect of coming through a portal.
    public void Spawn()
    {
        transform.position = LevelManager.Instance.StartPortal.transform.position; // sets the position where the monster will spawn.
        this.health.MaxVal = 10;
        this.health.CurrentVal = this.health.MaxVal;

        allowMovement = false; // disallows movement while the monster is scaling.

        StartCoroutine(Scale(new Vector3(0.1f, 0.1f), new Vector3(1, 1), false));// starts the scaling function alongside the spawn function.

        

        SetPath(LevelManager.Instance.Path); // sets the path that the monster will follow.
    }

    /// <summary>
    /// Function used to change the size of the monster
    /// </summary>
    /// <param name="from">Initial Size</param>
    /// <param name="to">Target Size</param>
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


    public void TakeDamage (float damage)
    {
        if (isActiveAndEnabled)
        {
            health.CurrentVal = Mathf.Clamp(health.CurrentVal - damage, 0, health.MaxVal) ;

            if (health.CurrentVal <= 0)
            {
                GameManager.Instance.Currency += 25;

                Release();
            }
        }
        

    }

    private void Release()
    {

        GameManager.Instance.Pool.ReleaseObject(gameObject);
        GameManager.Instance.RemoveMonster(this);
    }

    //Despawns the monster when they reach the goal.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Goal")
        {
            //Scale the monster down.
            StartCoroutine(Scale(new Vector3(1, 1), new Vector3(0.1f, 0.1f), true));
            GameManager.Instance.PlayerLives--;// take away one of the player lives upon a monster reaching the goal.
        }
        
    }

    private void Awake()
    {
        health.Initialize();
    }

    private void Update()
    {
        MoveMonster(); //calls the move monster function every tick.
    }
}
