using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TowerButton : MonoBehaviour
{
    [SerializeField]
    private GameObject towerPrefab;
    public GameObject TowerPrefab
    {
        get { return towerPrefab; }
    }

    private Tower tower;

    public Tower Tower
    {
        get
        {
            return tower;
        }

        private set
        {
            tower = value;
        }
    }


    [SerializeField]
    private TextMeshProUGUI priceText;


    private void Awake()
    {
        tower = towerPrefab.GetComponentInChildren<Tower>();
    }

    private void Start()
    {
        priceText.text = tower.Price.ToString();

        GameManager.Instance.CurrencyChange += new CurrencyChanged(PriceCheck);
    }

    /// <summary>
    /// Compares the price of a tower against the currency of the player. Towers that cannot be bought will have their buttons greyed out.
    /// </summary>
    private void PriceCheck()
    {
        if (tower.Price <= GameManager.Instance.Currency)
        {
            GetComponent<Image>().color = Color.white;
            priceText.color = new Color(233, 243, 0);

        }
        else
        {
            GetComponent<Image>().color = Color.grey;
            priceText.color = Color.grey;
        }
    }

    /// <summary>
    /// Display the level 1 stats of the tower when you hover on its icon. Probably should be modified/removed after converting to touch controls later. 
    /// </summary>
    public void ShowInfo ()
    {
        Tower.GetStats();
   
    }


}
