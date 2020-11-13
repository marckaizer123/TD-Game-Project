using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public TowerButton ClickedButton { get; private set; }

    public void PickTower(TowerButton towerButton)
    {
        this.ClickedButton = towerButton;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
