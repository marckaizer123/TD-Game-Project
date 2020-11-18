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

    private int wave = 0;

    [SerializeField]
    private TextMeshProUGUI waveText;

    [SerializeField]
    private GameObject waveButton;


    private List<Monster> activeMonsters = new List<Monster>();

    public bool waveIsActive
    {
        get
        {
            return activeMonsters.Count > 0;
        }

    }

    //Variable that holds the clicked button.
    public TowerButton ClickedButton { get; set; }

    public ObjectPool Pool { get; set; }

    /// <summary>
    /// Starts the wave when the Wave Button is pressed.
    /// </summary>
    public void StartWave()
    {
        wave++;

        waveText.text = string.Format("Wave: {0}", wave);

        StartCoroutine(SpawnWave());

        //Hide Wave Button when wave is ongoing
        waveButton.SetActive(false);
    }

    /// <summary>
    /// Spawns the wave.
    /// </summary>
    /// <returns></returns>
    private IEnumerator SpawnWave()
    {
        int waveSize = 3;
        int monsterIndex;


        if (wave == 1)
        {
            waveSize = wave;
        }
        else if (wave > 1 && wave<5)
        {
            waveSize = wave + 1;
        }
        else if (wave >= 5 && wave < 10)
        {
            waveSize = wave + 3;
        }

        


        string type = string.Empty;
        LevelManager.Instance.GeneratePath();


        ///Spawns an amount of monsters equal to waveSize.
        for (int i = 0; i < waveSize; i++)
        {
            monsterIndex = Random.Range(0, 3);

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

            activeMonsters.Add(monster);

            //delay between spawning of monsters.
            yield return new WaitForSeconds(1.0f);
        }

        
    }

    
    public void RemoveMonster(Monster monster)
    {
        activeMonsters.Remove(monster);

        if (!waveIsActive)
        {
            waveButton.SetActive(true);
        }
    }



    /// <summary>
    /// Function that selects which tower to place.
    /// </summary>
    /// <param name="towerButton"></param>
    public void PickTower(TowerButton towerButton)
    {

        if (Currency>= towerButton.Price && !waveIsActive)
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
