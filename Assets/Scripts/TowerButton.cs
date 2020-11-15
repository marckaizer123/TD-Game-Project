using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
    private TextMeshProUGUI priceText;


    private void Start()
    {
        priceText.text = Price + "$";
    }


}
