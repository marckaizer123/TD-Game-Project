using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Hover is used to allow the players to preview the tower they are going to place, its range, and whether or not they will be allowed to put it in a particular place
/// </summary>
public class Hover :Singleton<Hover>
{


    private void FollowMouse()
    {
        //Hover should only follow the mouse/drag when hover is activated.
        if (transform.GetChild(0).GetComponent<SpriteRenderer>().enabled && transform.GetChild(1).GetComponent<SpriteRenderer>().enabled) 
        {
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);

            transform.GetChild(0).position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.GetChild(0).position = new Vector3(transform.position.x, transform.position.y, 0);

            transform.GetChild(1).position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.GetChild(1).position = new Vector3(transform.position.x, transform.position.y, 0);
        }
        
    }


    public void Activate(GameObject towerPrefab)
    {
        //Shows the tower sprite of the selected tower.
        transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
        transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = towerPrefab.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
        transform.GetChild(0).transform.localScale = towerPrefab.transform.GetChild(0).transform.localScale;


        //Shows the range circle of the selected tower.
        transform.GetChild(1).GetComponent<SpriteRenderer>().enabled = true;
        transform.GetChild(1).transform.localScale = towerPrefab.transform.GetChild(1).transform.localScale;
        transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = towerPrefab.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite;
        transform.GetChild(1).GetComponent<SpriteRenderer>().color = towerPrefab.transform.GetChild(1).GetComponent<SpriteRenderer>().color;


    }

    public void Deactivate()
    {

        transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
        transform.GetChild(1).GetComponent<SpriteRenderer>().enabled = false;

        GameManager.Instance.ClickedButton = null;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        FollowMouse();
    }
}
