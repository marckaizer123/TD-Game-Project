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

    public void ShowInfo (string type)
    {
        GameManager.Instance.SetToolTipText(type);
        GameManager.Instance.ShowStats();
    }


}
