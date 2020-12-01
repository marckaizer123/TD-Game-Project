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

    private int wave;
    private int waveSize;
    private int maxMonsterIndex;
    private int bonusHealth;


    [SerializeField]
    private TextMeshProUGUI waveText;

    [SerializeField]
    private GameObject waveButton;

    [SerializeField]
    private GameObject cancelField;


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

    [SerializeField]
    private TextMeshProUGUI upgradeText;
    


    /// <summary>
    /// Starts the wave when the Wave Button is pressed. Will also update the parameters of the wave it will spawn.
    /// </summary>
    public void StartWave()
    {
        wave++;

        
        waveSize++;

        if (wave % 5 == 0)
        {
            waveSize += 1;
        }

        if (wave % 10 == 0)
        {
            waveSize += 1;
           

        }

        if (wave == 3)
        {
            maxMonsterIndex++;
        }

        if (wave == 6)
        {
            maxMonsterIndex++;
        }

        if (wave == 9)
        {
            maxMonsterIndex++;
        }

        if (wave % 5 == 0)
        {
            bonusHealth = 5;
        }
        else
        {
            bonusHealth = 0;
        }




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
        int monsterIndex;


        string type = string.Empty;
        LevelManager.Instance.GeneratePath();


        ///Spawns an amount of monsters equal to waveSize.
        for (int i = 0; i < waveSize; i++)
        {
            monsterIndex = Random.Range(0, maxMonsterIndex);

            switch (monsterIndex)
            {
                case 0:
                    type = "TiyanakPrefab";
                    break;

                case 1:
                    type = "GhostPrefab";
                    break;

                case 2:
                    type = "SlimePrefab";
                    break;

                case 3:
                    type = "SkeletonPrefab";
                    break;
            }

            Monster monster = Pool.GetObject(type).GetComponent<Monster>();
            monster.Spawn(bonusHealth);

            //add the monster to the list of active monsters
            activeMonsters.Add(monster);

            //delay between spawning of monsters.
            yield return new WaitForSeconds(1.0f);
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
            cancelField.SetActive(true);
        }
        
    }

    /// <summary>
    /// Function that allows the players to cancel their picked tower.
    /// </summary>
    public void CancelTowerPick()
    {
        ClickedButton = null;

        Hover.Instance.Deactivate();
        cancelField.SetActive(false);
        ShowStats();
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
            cancelField.SetActive(false);
            ShowStats();
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
            DeselectTower();        
        }


    }

    public void SelectTower(Tower tower)
    {
        if (selectedTower != null)
        {
            selectedTower.Select();

        }

        //Sets the selected tower.
        selectedTower = tower;

        //Selects the Tower
        selectedTower.Select();

        selectedTower.GetStats();

        gamePanel.SetActive(true);
        statsPanel.SetActive(true);

        sellText.text = "Sell : " + selectedTower.Price / 2;

        if(selectedTower.NextUpgrade != null)
        {
            upgradeText.text = "Upgrade: " + selectedTower.NextUpgrade.Price;
        }

        else
        {
            upgradeText.text = "Max Upgrade Reached";
        }


        CheckUpgrade();


    }

    public void DeselectTower()
    {
        if (selectedTower != null)
        {
            selectedTower.Select();
        }

        selectedTower = null;

        gamePanel.SetActive(false);

        statsPanel.SetActive(false);
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
    }

    public void UpgradeTower()
    {
        if (selectedTower!= null)
        {
            if (selectedTower.NextUpgrade!=null && Currency >= selectedTower.NextUpgrade.Price)
            {

                selectedTower.Upgrade();

                if (selectedTower.NextUpgrade != null)
                {
                    upgradeText.text = "Upgrade: " + selectedTower.NextUpgrade.Price;
                }

                else
                {
                    upgradeText.text = "Max Upgrade Reached";
                }

                CheckUpgrade();

                SetToolTipText(selectedTower.SetTooltip());

                sellText.text = "Sell : " + selectedTower.Price / 2;                                         
            }
        }
    }

    /// <summary>
    /// Greys out the upgrade button if player can't afford it.
    /// </summary>
    public void CheckUpgrade()
    {
        if (selectedTower.NextUpgrade == null)
        {
            gamePanel.transform.GetChild(1).GetComponent<Image>().color = Color.grey;
        }

        else if (currency >= selectedTower.NextUpgrade.Price)
        {
            gamePanel.transform.GetChild(1).GetComponent<Image>().color = Color.white;
        }

        else
        {
            gamePanel.transform.GetChild(1).GetComponent<Image>().color = Color.grey;
        }
    }

    /// <summary>
    /// Game Over code
    /// </summary>
    public void GameOver()
    {
        if (!gameOver)
        {
            AudioManager.Instance.PlaySFX("GameOver");
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

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
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

        waveSize = 0;
        wave = 0;
        maxMonsterIndex = 1;
    }

    // Start is called before the first frame update
    void Start()
    {
        PlayerLives = 20;
        Currency = 500;
    }

    // Update is called once per frame
    void Update()
    {
        EscapeSelection();
    }
}
