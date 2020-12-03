using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField]
    private GameObject[] tilePrefabs;

    [SerializeField]
    private MoveCamera moveCamera;

    [SerializeField]
    private Transform map;


    private Point startSpawn;
    private Point goalSpawn;


    [SerializeField]
    private GameObject startPrefab;
    [SerializeField]
    private GameObject goalPrefab;

    private Stack<Node> path;

    public Stack<Node> Path
    {
        get
        {
            if(path == null)
            {
                GeneratePath();
            }
            return new Stack<Node>(new Stack<Node>(path));
        }
    }

    public Portal StartPortal { get; set; }
    public Portal GoalPortal { get; set; }

    /// <summary>
    /// A dictionary that contains all the tiles in the game.
    /// </summary>
    public Dictionary<Point, TileScript> Tiles { get; set; }


    public float TileSize //Gets the width of the tile.
    {
        get
        {
            return tilePrefabs[0].GetComponent<SpriteRenderer>().sprite.bounds.size.x;
        }
    }

    private void PlaceTile(string tileType, int x, int y, Vector3 worldStart) //Function for placing a tile at a specific location, x is the x coordinate, y is the y coordinate, and worldStart is the start point.
    {
        //Changes tileType into an integer so that it can be used as an index when we create a new tile.
        int tileIndex = int.Parse(tileType);

        //Creates a new tile and makes a reference to that tile using the newTile variable.
        TileScript newTile = Instantiate(tilePrefabs[tileIndex]).GetComponent<TileScript>();

        //Sets the position of the tile.
        newTile.Setup(new Point(x, y), new Vector3(worldStart.x + (TileSize * x), worldStart.y - (TileSize * y), 0), map);

        //if Tile is a grassTile, then sets AllowsTower to true and Walkable to false.
        if(tileIndex == 0)
        { 
            newTile.AllowsTower = true;
            newTile.Walkable = false;
        }
        //if Tile is a dirtTile, then sets AllowsTower to false and Walkable to true.
        if(tileIndex == 2)
        {
            newTile.AllowsTower = false;
            newTile.Walkable = true;
        }  
    }

    /// <summary>
    /// Function that places the portals.
    /// </summary>
    private void SpawnPortal()
    {
        startSpawn = new Point(1, 2);
        //goalSpawn = new Point(1, 5); for testing purposes
        goalSpawn = new Point(22, 2);

        //Player cannot place a tower on the tiles that are covered by the starting portal.
        for (int x = -1; x <= 1; x++)
        {
            for (int y = 0; y <= 2; y++)
            {
                Point point = new Point(startSpawn.X - x, startSpawn.Y - y);

                Tiles[point].AllowsTower = false;
            }

        }

        //Player cannot place a tower on the tiles that are covered by the goal portal.
        for (int x = -1; x <= 1; x++)
        {
            for (int y = 0; y <= 2; y++)
            {
                Point point = new Point(goalSpawn.X - x, goalSpawn.Y - y);

                Tiles[point].AllowsTower = false;
            }

        }


        GameObject tmp = (GameObject)Instantiate(startPrefab, Tiles[startSpawn].GetComponent<TileScript>().WorldPosition, Quaternion.identity);

        StartPortal = tmp.GetComponent<Portal>();
        StartPortal.name = "Start";

        tmp = (GameObject)Instantiate(goalPrefab, Tiles[goalSpawn].GetComponent<TileScript>().WorldPosition, Quaternion.identity);

        
        GoalPortal = tmp.GetComponent<Portal>();
        GoalPortal.name = "Goal";
    }

    /// <summary>
    /// Reads the Level.txt file and convert it to an array of strings.
    /// </summary>
    /// <returns></returns>
    private string[] ReadLevelText()
    {
        TextAsset bindData = Resources.Load("Level") as TextAsset;

        string data = bindData.text.Replace(Environment.NewLine, string.Empty);

        return data.Split('-');
    }

    private void createLevel()
    {

        Tiles = new Dictionary<Point, TileScript>();

        //Defines the size of the map and the tiles that are at certain positions
        string[] mapData = ReadLevelText();


        //Converts the mapData into x and y coordinates
        int mapX = mapData[0].ToCharArray().Length;
        int mapY = mapData.Length;

        Vector3 minTile;
        Vector3 maxTile;

        //Calculates the world's start point.
        Vector3 worldStart = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height));

        for (int y = 0; y < mapY; y++) //The y positions of the tiles
        {

            char[] newTiles = mapData[y].ToCharArray();

            for (int x = 0; x < mapX; x++) // The x positions of the tiles
            {
                //Places the tile in the world
                PlaceTile(newTiles[x].ToString(),
                          x,
                          y,
                          worldStart);
            }
        }


        minTile = Tiles[new Point(0, 0)].transform.position; // find the very first tile placed.
        maxTile = Tiles[new Point(mapX - 1, mapY - 1)].transform.position; // find the very last tile placed.


        moveCamera.SetMinLimits(new Vector3(minTile.x - TileSize, minTile.y + TileSize)); //sets the minimum limits of the camera using the first tile placed.

        moveCamera.SetMaxLimits(new Vector3(maxTile.x + 2*TileSize, maxTile.y - 2*TileSize, -10)); // sets the max limits of the camera using the last tile placed.

        SpawnPortal(); // spawns th portals above the tiles.
    }

    public void GeneratePath()
    {
        path = AStar.GetPath(startSpawn, goalSpawn);
    }

    // Start is called before the first frame update
    void Start()
    {
        createLevel(); 
    }

    
}
