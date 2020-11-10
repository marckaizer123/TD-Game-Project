using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{

    public GameObject testEnemyPrefab;
    public GameObject[] waypoints;

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(testEnemyPrefab).GetComponent<MoveEnemy>().waypoints = waypoints;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
