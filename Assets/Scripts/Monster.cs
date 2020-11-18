using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [SerializeField]
    private float speed = 0;

    private Stack<Node> path;

    public Point GridPosition { get; set; }

    private Vector3 destination;

    /// <summary>
    /// Moves the monster
    /// </summary>
    private void MoveMonster()
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

    /// <summary>
    /// Rotates the monster based on the direction it will move into.
    /// </summary>
    private void RotateMonster()
    {
        Vector3 faceDirection = destination - transform.position;

        float rotationAngle = Mathf.Atan2(faceDirection.y, faceDirection.x) * 180 / Mathf.PI;

        transform.rotation = Quaternion.AngleAxis(rotationAngle, Vector3.forward);
    }

    /// <summary>
    /// Sets the path that the monster will follow.
    /// </summary>
    private void SetPath(Stack<Node> newPath)
    {
        if(newPath != null)
        {
            this.path = newPath;

            GridPosition = path.Peek().GridPosition;
            destination = path.Pop().WorldPosition;
        }
    }

    //Spawns the monsters. Uses scale to make the monster start from a smaller size then scale into a bigger size to give the effect of coming through a portal.
    public void Spawn()
    {
        transform.position = LevelManager.Instance.StartPortal.transform.position;

        StartCoroutine(Scale(new Vector3(0.1f, 0.1f), new Vector3(1, 1), false));

        SetPath(LevelManager.Instance.Path);
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

        if(remove == true)
        {
            Release();
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
            StartCoroutine(Scale(new Vector3(1, 1), new Vector3(0.1f, 0.1f), true));
        }
    }

    private void Update()
    {
        MoveMonster();
    }
}
