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



    public TowerButton ClickedButton { get; private set; }

    public void PickTower(TowerButton towerButton)
    {

        if (Currency>= towerButton.Price)
        {
            this.ClickedButton = towerButton;
        }
        
    }

    public void BuyTower()
    {
        if (Currency>= ClickedButton.Price)
        {
            Currency -= ClickedButton.Price;
            ClickedButton = null;
        }
        
    }


    // Start is called before the first frame update
    void Start()
    {
        Currency = 500;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
