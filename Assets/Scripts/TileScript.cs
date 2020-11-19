using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileScript : MonoBehaviour
{
    public Point GridPosition { get; set; }

    public Vector2 WorldPosition
    {
        get
        {
            return GetComponent<SpriteRenderer>().bounds.center;
        }
    }

    private SpriteRenderer spriteRenderer;


    private Color32 fullColor = new Color32(255, 118, 118, 255);

    private Color32 emptyColor = new Color32(96, 255, 90, 255);

    public bool IsEmpty { get; private set; }
    public bool AllowsTower { get; set; }

    public bool Walkable { get; set; }

    public bool Debugging { get; set; }


    private Tower myTower;


    private void ColorTile(Color32 newColor)
    {
        spriteRenderer.color = newColor;
    }


    /// <summary>
    /// Sets up the tile.
    /// </summary>
    /// <param name="gridPos">The grid position of the tile</param>
    /// <param name="worldPos">The world position of the tile</param>
    public void Setup(Point gridPos, Vector3 worldPos, Transform parent)
    {

        IsEmpty = true;
        this.GridPosition = gridPos;
        transform.position = worldPos;
        transform.SetParent(parent);
        LevelManager.Instance.Tiles.Add(gridPos, this);
    }

    private void PlaceTower()
    {       
        GameObject tower = (GameObject)Instantiate(GameManager.Instance.ClickedButton.TowerPrefab, WorldPosition, Quaternion.identity);

        tower.GetComponent<SpriteRenderer>().sortingOrder = GridPosition.Y + 1;
        tower.transform.SetParent(transform);

        this.myTower = tower.transform.GetChild(0).GetComponent<Tower>();


        IsEmpty = false;

        GameManager.Instance.BuyTower();

    }

    /// <summary>
    /// Convert to touch controls later
    /// </summary>
    private void OnMouseOver()
    {
        if (!EventSystem.current.IsPointerOverGameObject() && GameManager.Instance.ClickedButton != null)
        {
            if (AllowsTower)
            {
                if (IsEmpty)
                {
                    ColorTile(emptyColor); //Changes the color of the tile that is under the mouse when a tower is selected.

                    if (Input.GetMouseButtonDown(0)) //Allows placement of tower.
                    {
                        PlaceTower();
                        Debug.Log("Click");
                    }
                }

                else
                {
                    ColorTile(fullColor);

                }
            }
        }

        else if (!EventSystem.current.IsPointerOverGameObject() && GameManager.Instance.ClickedButton == null && Input.GetMouseButtonDown(0))
        {
            if(myTower != null)
            {
                GameManager.Instance.SelectTower(myTower);
            }
            else
            {
                GameManager.Instance.DeselectTower();
            }
        }
        
    }

    /// <summary>
    /// Convert to mouse controls later.
    /// </summary>
    private void OnMouseExit()
    {
        if (!Debugging)
        {
            ColorTile(Color.white);
        }
        
    }

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
