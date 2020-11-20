using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

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

    private int playerLives;

    public int PlayerLives
    {
        get { return playerLives; }
        set 
        {

            this.playerLives = value;

            if (playerLives <= 0)
            {
                this.playerLives = 0;
                GameOver();
            }
            
            this.playerLivesText.text = "Lives: " + playerLives.ToString();
        }
    }

    [SerializeField]
    private TextMeshProUGUI playerLivesText;

    private int wave = 0;
    private int waveSize = 0;


    [SerializeField]
    private TextMeshProUGUI waveText;

    [SerializeField]
    private GameObject waveButton;


    private List<Monster> activeMonsters = new List<Monster>();

    public bool waveIsActive
    {
        get
        {
            return activeMonsters.Count > 0; // the wave is considered active so long as the monster game objects are still active.
        }

    }

    //Variable that holds the clicked button.
    public TowerButton ClickedButton { get; set; }

    public ObjectPool Pool { get; set; }


    private bool gameOver = false;

    [SerializeField]
    private GameObject gameOverMenu;

    private Tower selectedTower;


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

        waveSize++;

        if (wave % 5 == 0)
        {
            waveSize += 2;
        }


        int monsterIndex;

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
            yield return new WaitForSeconds(0.5f);
        }

        
    }

    
    public void RemoveMonster(Monster monster)
    {
        activeMonsters.Remove(monster);

        if (!waveIsActive && !gameOver)
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

        if (Currency>= towerButton.Price && !waveIsActive && !gameOver)
        {
            this.ClickedButton = towerButton;
            Hover.Instance.Activate(towerButton.TowerPrefab);
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

    public void SelectTower(Tower tower)
    {
        if(selectedTower != null)
        {
            selectedTower.Select();
        }
        selectedTower = tower;
        selectedTower.Select();
    }

    public void DeselectTower()
    {
        if (selectedTower != null)
        {
            selectedTower.Select();
        }

        selectedTower = null;
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

    /// <summary>
    /// Game Over code
    /// </summary>
    public void GameOver()
    {
        if (!gameOver)
        {
            gameOver = true;
            gameOverMenu.SetActive(true);
        }
    }

    public void Restart()
    {

        Time.timeScale = 1;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    // Awake is called on loading the script
    private void Awake()
    {
        Pool = GetComponent<ObjectPool>();
    }

    // Start is called before the first frame update
    void Start()
    {
        PlayerLives = 10;
        Currency = 500;
    }

    // Update is called once per frame
    void Update()
    {
        EscapeSelection();
    }
}
