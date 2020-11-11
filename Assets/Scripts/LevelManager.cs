using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private GameObject tile;

    public float TileSize
    {
        get
        {
            return tile.GetComponent<SpriteRenderer>().sprite.bounds.size.x;
        }
    }

    private void PlaceTile(int x, int y, Vector3 worldStart)
    {
        GameObject newtile = Instantiate(tile);
        newtile.transform.position = new Vector3(worldStart.x + (TileSize * x), worldStart.y - (TileSize * y), 0);
    }


    private void createLevel()
    {
        Vector3 worldStart = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height));

        for (int y = 0; y < 5; y++)
        {
            for (int x = 0; x < 5; x++)
            {
                PlaceTile(x, y, worldStart);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        createLevel(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
