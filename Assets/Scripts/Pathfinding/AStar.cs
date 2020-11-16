using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AStar
{
    //Dictionary that will contain nodes.
    private static Dictionary<Point, Node> nodes;



    /// <summary>
    /// Creates a node for each TileScript and adds them to the dictionary.
    /// </summary>
    private static void CreateNodes()
    {
        
        nodes = new Dictionary<Point, Node>();

        //Add nodes to node dictionary
        foreach (TileScript tile in LevelManager.Instance.Tiles.Values)
        {
            nodes.Add(tile.GridPosition, new Node(tile));
        }
    }

    public static void GetPath(Point start)
    {
        if (nodes == null)
        {
            CreateNodes();
        }

        HashSet<Node> openList = new HashSet<Node>();

        //find the start node and creates a reference to it called currentNode.
        Node currentNode = nodes[start];

        //add starting node to openlist
        openList.Add(currentNode);

        //Finds the surrounding tiles from the currentNode.
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                Point neighborPos = new Point(currentNode.GridPosition.X - x, currentNode.GridPosition.Y - y);

                
                if (nodes.ContainsKey(neighborPos))
                    //checks if the tile is in the nodes dictionary
                {
                    if (LevelManager.Instance.Tiles[neighborPos].Walkable && neighborPos != currentNode.GridPosition)
                    //checks if the neighboring tiles are walkable and ignores the currentNode.
                    {
                        Node neighbor = nodes[neighborPos];

                        //Only for debugging, remove later
                        neighbor.TileRef.SpriteRenderer.color = Color.black;
                    }
                }

                
                
                

                

            }
        }

        //For debugging only, remove later
        GameObject.Find("Debugger").GetComponent<Debugger>().DebugPath(openList);

    }
}
