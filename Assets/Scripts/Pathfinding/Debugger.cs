using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debugger : MonoBehaviour
{

    private TileScript start, goal;
    [SerializeField]
    private Sprite blankTile;

    [SerializeField]
    private GameObject debugTilePrefab;

    [SerializeField]
    private GameObject arrowPrefab;

    private void PlaceDebugTile(Vector3 worldPos, Color32 color, Node node = null)
    {
        GameObject debugTile = (GameObject)Instantiate(debugTilePrefab, worldPos, Quaternion.identity);

        debugTile.GetComponent<SpriteRenderer>().color = color;

        if(node != null)
        {
            DebugTile tmp = debugTile.GetComponent<DebugTile>();

            tmp.G.text += node.G;
            tmp.H.text += node.H;
            tmp.F.text += node.F;
        }
    }

    private void ClickTile()
    {
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider != null)
            {
                TileScript tmp = hit.collider.GetComponent<TileScript>();

                if  (tmp != null)
                {
                    if (start == null)
                    {
                        start = tmp;
                        PlaceDebugTile(start.WorldPosition, new Color32(255, 135, 0, 255));
                        
                    }
                    else if (goal == null)
                    {
                        goal = tmp;
                        PlaceDebugTile(goal.WorldPosition, new Color32(255, 0, 0, 255));

                    }
                }
            }
        }
    }



    private void PointToParent(Node node, Vector2 position)
    {

        if (node.Parent != null)
        {
            GameObject arrow = (GameObject)Instantiate(arrowPrefab, position, Quaternion.identity);

            float x = node.Parent.GridPosition.X - node.GridPosition.X ;
            float y = node.GridPosition.Y - node.Parent.GridPosition.Y ;
            float rotationAngle = Mathf.Atan2(y, x) * 180 / Mathf.PI;
            arrow.transform.rotation = Quaternion.AngleAxis(rotationAngle, Vector3.forward);
            
        }
    }

    public void DebugPath(HashSet<Node> openList, HashSet<Node> closedList, Stack<Node> path)
    {
        foreach(Node node in openList)
        {
            

            if (node.TileRef != start && node.TileRef != goal)
            {
                PlaceDebugTile(node.TileRef.WorldPosition, Color.cyan, node);
            }

            PointToParent(node, node.TileRef.WorldPosition);
        }

        foreach (Node node in closedList)
        {


            if (node.TileRef != start && node.TileRef != goal && !path.Contains(node))
            {
                PlaceDebugTile(node.TileRef.WorldPosition, Color.green, node);
            }
            PointToParent(node, node.TileRef.WorldPosition);
        }

        foreach (Node node in path)
        {
            if (node.TileRef != start && node.TileRef != goal)
            {
                PlaceDebugTile(node.TileRef.WorldPosition, Color.blue, node);
            }
        }
    }

    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ClickTile();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            AStar.GetPath(start.GridPosition, goal.GridPosition);
        }
    }
}
