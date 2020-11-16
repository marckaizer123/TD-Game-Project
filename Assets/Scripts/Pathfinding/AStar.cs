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

        Node currentNode = nodes[start];

        openList.Add(currentNode);

        //For debugging only, remove later
        GameObject.Find("Debugger").GetComponent<Debugger>().DebugPath(openList);

    }
}
