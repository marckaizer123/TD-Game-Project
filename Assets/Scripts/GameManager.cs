using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : Singleton<GameManager>
{

    private int currency;
    public int Currency
    {
        get 
        { return currency; 

        }
        set 
        { this.currency = value;
          this.currencyText.text = value.ToString() + "$";
        }
    }

    [SerializeField]
    private TextMeshProUGUI currencyText;


    //Variable that holds the clicked button.
    public TowerButton ClickedButton { get; set; }

    public ObjectPool Pool { get; set; }


    

    private IEnumerator SpawnWave()
    {

        int monsterIndex = Random.Range(0, 3);

        string type = string.Empty;

        switch (monsterIndex)
        {
            case 0:
            type = "MonsterPrefab1";
            break;

            case 1:
            type = "MonsterPrefab1";
            break;

            case 2:
            type = "MonsterPrefab1";
            break;

            case 3:
            type = "MonsterPrefab1";
            break;

        }

        Monster monster = Pool.GetObject(type).GetComponent<Monster>();
        monster.Spawn();

        //delay between spawning of monsters.
        yield return new WaitForSeconds(2.5f);
    }

    public void StartWave()
    {
        StartCoroutine(SpawnWave());
    }


    /// <summary>
    /// Function that selects which tower to place.
    /// </summary>
    /// <param name="towerButton"></param>
    public void PickTower(TowerButton towerButton)
    {

        if (Currency>= towerButton.Price)
        {
            this.ClickedButton = towerButton;
            Hover.Instance.Activate(towerButton.Sprite);
        }
        
    }


    /// <summary>
    /// Function to finish buying a tower. 
    /// It subtracts the tower price from the player's currency, removes the hovering icon, and sets the selection back to null.
    /// </summary>
    public void BuyTower()
    {
        if (Currency>= ClickedButton.Price)
        {
            Currency -= ClickedButton.Price;
            Hover.Instance.Deactivate();
            
        }
        
    }

    /// <summary>
    /// Function to cancel selection of a tower. Convert to Touch controls later.
    /// </summary>
    private void EscapeSelection()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Hover.Instance.Deactivate();
        }
    }


    // Awake is called on loading the script
    private void Awake()
    {
        Pool = GetComponent<ObjectPool>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Currency = 500;
    }

    // Update is called once per frame
    void Update()
    {
        EscapeSelection();
    }
}
