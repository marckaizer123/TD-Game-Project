using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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

    public static Stack<Node> GetPath(Point start, Point goal)
    {
        if (nodes == null)
        {
            CreateNodes();
        }

        //Creates an open list to use for the search algorithm
        HashSet<Node> openList = new HashSet<Node>();

        //Creates an closed list to use for the search algorithm
        HashSet<Node> closedList = new HashSet<Node>();

        //Creates a stack that will hold the final path.
        Stack<Node> finalPath = new Stack<Node>();

        //Find the start node and creates a reference to it called currentNode.
        Node currentNode = nodes[start];

        //Add the starting node to the open list
        openList.Add(currentNode);

        while (openList.Count > 0)
        {
            //Search for the neighboring nodes.
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    Point neighborPos = new Point(currentNode.GridPosition.X - x, currentNode.GridPosition.Y - y);

                    //checks if the tile is in the nodes dictionary and if the neighboring tiles are walkable. Ignores the current node and neigbors that are diagonal to the current node.
                    if (nodes.ContainsKey(neighborPos) && LevelManager.Instance.Tiles[neighborPos].Walkable && neighborPos != currentNode.GridPosition && Mathf.Abs(x - y) == 1)
                    {
                        int gCost = 10;

                        Node neighbor = nodes[neighborPos];


                        if (openList.Contains(neighbor))
                        {
                            if (currentNode.G + gCost < neighbor.G)
                            {
                                neighbor.CalcValues(currentNode, nodes[goal], gCost);
                            }
                        }

                        else if (!closedList.Contains(neighbor))
                        {
                            openList.Add(neighbor);
                            neighbor.CalcValues(currentNode, nodes[goal], gCost);
                        }
                    }                
                }
            }

            //Move the current node from open list to the closed list.
            openList.Remove(currentNode);
            closedList.Add(currentNode);

            if (openList.Count > 0)
            {
                //sorts the list by F then selects the first on the list
                currentNode = openList.OrderBy(n => n.F).First();
            }

            if (currentNode == nodes[goal])
            {
                while (currentNode.GridPosition != start)
                {
                    finalPath.Push(currentNode);
                    currentNode = currentNode.Parent;
                }

                break;
            }
        }

        return finalPath;

        //For debugging only, remove later
        //GameObject.Find("Debugger").GetComponent<Debugger>().DebugPath(openList, closedList, finalPath);

    }
}
