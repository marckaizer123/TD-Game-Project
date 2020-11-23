using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

/// <summary>
/// Delegate for the currency changed event
/// </summary>
public delegate void CurrencyChanged();

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
          this.currencyText.text = value.ToString();

          OnCurrencyChange();
        }
    }

    [SerializeField]
    private TextMeshProUGUI currencyText;

    //event that triggers when currency is changed.
    public event CurrencyChanged CurrencyChange;

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
            
            this.playerLivesText.text = playerLives.ToString();
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


    [SerializeField]
    private GameObject statsPanel;

    [SerializeField]
    private TextMeshProUGUI sizeText;

    [SerializeField]
    private TextMeshProUGUI visibleText;

    private bool gameOver = false;


    [SerializeField]
    private GameObject gameOverMenu;

    [SerializeField]
    private GameObject ingameMenu;

    [SerializeField]
    private GameObject pauseMenu;

    [SerializeField]
    private GameObject optionsMenu;

    [SerializeField]
    private GameObject gamePanel;

    




    private Tower selectedTower;

    [SerializeField]
    private TextMeshProUGUI sellText;


    


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
                    type = "TiyanakPrefab";
                    break;

                case 1:
                    type = "TiyanakPrefab";
                    break;

                case 2:
                    type = "TiyanakPrefab";
                    break;

                case 3:
                    type = "TiyanakPrefab";
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
    /// Only allowed to finish buying when player has enough currency
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

    //Upon player currency changing.
    public void OnCurrencyChange()
    {
        if (CurrencyChange != null)
        {
            CurrencyChange();
        }
    }

    //Function to sell tower.
    public void SellTower()
    {
        if (selectedTower != null && !waveIsActive && !gameOver)
        {
            Currency += selectedTower.Price / 2;

            Destroy(selectedTower.transform.parent.gameObject);
            selectedTower.GetComponentInParent<TileScript>().IsEmpty = true;
            DeselectTower();        }
    }

    public void SelectTower(Tower tower)
    {
        if(selectedTower != null)
        {
            selectedTower.Select();
        }

        //Sets the selected tower.
        selectedTower = tower;

        //Selects the Tower
        selectedTower.Select();

        gamePanel.SetActive(true);

        sellText.text = "Sell : " + selectedTower.Price/2;
    }

    public void DeselectTower()
    {
        if (selectedTower != null)
        {
            selectedTower.Select();
        }

        selectedTower = null;
        gamePanel.SetActive(false);
    }

    /// <summary>
    /// Function to cancel selection of a tower. Convert to Touch controls later.
    /// </summary>
    private void EscapeSelection()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Hover.Instance.Deactivate();


            ShowInGameMenu();
        }
    }

    public void ShowStats()
    {
        statsPanel.SetActive(!statsPanel.activeSelf);
    }

    public void SetToolTipText(string txt)
    {
        visibleText.text = txt;
        sizeText.text = txt;
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

    public void ShowInGameMenu()
    {
       ingameMenu.SetActive(!ingameMenu.activeSelf);



        if (!ingameMenu.activeSelf)
        {
            Time.timeScale = 1;
        }
        else
        {
            ShowPauseMenu();
            Time.timeScale = 0;
        }
    }

    public void ShowPauseMenu()
    {
        pauseMenu.SetActive(true);
    }

    public void ShowOptionsMenu()
    {
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }
    public void Resume()
    {

        ShowInGameMenu();
    }

    
    public void Restart()
    {

        Time.timeScale = 1;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Back()
    {
        optionsMenu.SetActive(false);
        pauseMenu.SetActive(true);      
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
