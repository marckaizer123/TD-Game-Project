using UnityEngine;
using UnityEngine.EventSystems;


/// <summary>
/// Tilescript holds all the behavior of a tile.
/// </summary>
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

    public bool IsEmpty { get; set; }
    public bool AllowsTower { get; set; }

    public bool Walkable { get; set; }


    public bool PointerOverGameOBject
    {
        get
        {
            if (EventSystem.current.IsPointerOverGameObject(-1) || EventSystem.current.IsPointerOverGameObject(0) || EventSystem.current.IsPointerOverGameObject(1) || EventSystem.current.IsPointerOverGameObject(2) || EventSystem.current.IsPointerOverGameObject(3))
            {
                return true;
            }

            else
            {
                return false;
            }
        }
    }


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

        IsEmpty = true; // sets the tile to be empty.
        this.GridPosition = gridPos;
        this.transform.position = worldPos;
        this.transform.SetParent(parent);
        LevelManager.Instance.Tiles.Add(gridPos, this);
    }

    private void PlaceTower()
    {   
        
        // instantiates a game object called tower.
        GameObject tower = Instantiate(GameManager.Instance.ClickedButton.TowerPrefab, //The game object placed will be equal to the tower prefab set by the clicked button.
                                       WorldPosition, // the tower will be instantiated at the grid position it was placed in.
                                       Quaternion.identity);

        tower.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = GridPosition.Y + 1;
        tower.transform.SetParent(transform);

        this.myTower = tower.GetComponentInChildren<Tower>();


        IsEmpty = false;


        GameManager.Instance.BuyTower();

    }

    /// <summary>
    /// Convert to touch controls later
    /// </summary>
    private void OnMouseOver()
    {
        if (!PointerOverGameOBject && GameManager.Instance.ClickedButton != null)
        {
            if (AllowsTower) // checks if the tile allows you to place the tower
            {
                if (IsEmpty)// checks if the tile is empty
                {
                    ColorTile(emptyColor); //Changes the color of the tile to indicate that it is empty.

                    if (Input.GetMouseButtonUp(0)) //Allows placement of tower.
                    {
                        PlaceTower();
                    }
                }

                else
                {
                    ColorTile(fullColor); //changes the color of the tile to indicate that it is occupied.

                    if (Input.GetMouseButtonUp(0)) // cancels the placement of tower if drag is released on a tile already has a tower.
                    {
                        GameManager.Instance.CancelTowerPick();
                    }

                }
            }

            //cancels the placement of tower if drag is released on a tile that does not allow towers.
            else 
            {
                if (Input.GetMouseButtonUp(0))
                {
                    GameManager.Instance.CancelTowerPick();
                }
                    
            }
        }

        else if(PointerOverGameOBject && GameManager.Instance.ClickedButton != null)
        {
            if (Input.GetMouseButtonUp(0)) // cancels the placement of tower if drag is released on a tile already has a tower.
            {
                GameManager.Instance.CancelTowerPick();
            }
        }

        else if (!PointerOverGameOBject && GameManager.Instance.ClickedButton == null && Input.GetMouseButtonDown(0))
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


    private void OnMouseExit()
    {
            ColorTile(Color.white);

    }

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

}
