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

    [SerializeField]
    private int price;

    public int Price
    {
        get { return price; }
    }

    [SerializeField]
    private Sprite sprite;

    public Sprite Sprite
    {
        get
        {
            return sprite;
        }
    }

    [SerializeField]
    private TextMeshProUGUI priceText;


    private void Start()
    {
        priceText.text = Price.ToString();

        GameManager.Instance.CurrencyChange += new CurrencyChanged(PriceCheck);
    }

    /// <summary>
    /// Compares the price of a tower against the currency of the player. Towers that cannot be bought will have their buttons greyed out.
    /// </summary>
    private void PriceCheck()
    {
        if (price <= GameManager.Instance.Currency)
        {
            GetComponent<Image>().color = Color.white;

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
        towerPrefab.GetComponentInChildren<Tower>().GetStats();
   
    }


}
