﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileScript : MonoBehaviour
{
    public Point GridPosition { get; private set; }

    public Vector2 WorldPosition
    {
        get
        {
            return GetComponent<SpriteRenderer>().bounds.center;
        }
    }



    /// <summary>
    /// Sets up the tile.
    /// </summary>
    /// <param name="gridPos">The grid position of the tile</param>
    /// <param name="worldPos">The world position of the tile</param>
    public void Setup(Point gridPos, Vector3 worldPos, Transform parent)
    {
        this.GridPosition = gridPos;

        transform.position = worldPos;

        transform.SetParent(parent);
        LevelManager.Instance.Tiles.Add(gridPos, this);
    }

    private void PlaceTower()
    {

        if (!EventSystem.current.IsPointerOverGameObject() && GameManager.Instance.ClickedButton!=null)
        {
            GameObject tower = (GameObject)Instantiate(GameManager.Instance.ClickedButton.TowerPrefab, WorldPosition, Quaternion.identity);

            tower.GetComponent<SpriteRenderer>().sortingOrder = GridPosition.Y + 1;

            tower.transform.SetParent(transform);
        }
        
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            PlaceTower();
        }
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
