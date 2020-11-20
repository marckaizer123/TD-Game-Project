using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Object pool is used to prevent instantiating and destroying game objects each tick and save on memory. 
/// </summary>
public class ObjectPool : MonoBehaviour
{
    [SerializeField]
    private GameObject[] objectPrefabs;

    private List<GameObject> pooledObjects = new List<GameObject>();

    public GameObject GetObject(string type)
    {
        //search the pool for inactive game objects to be reused.
        foreach (GameObject go in pooledObjects)
        {
            if(go.name == type && !go.activeInHierarchy)
            {
                go.SetActive(true);
                return go;

            }
        }

        //If the pool doesn't contain the object needed, create the object
        for (int i = 0; i < objectPrefabs.Length; i++)
        {
            //find the prefab for creating the object
            if (objectPrefabs[i].name == type)
            {
                GameObject newObject = Instantiate(objectPrefabs[i]);
                pooledObjects.Add(newObject);
                newObject.name = type;
                return newObject;
            }
        }

        return null;
    }

    public void ReleaseObject(GameObject gameObject)
    {
        gameObject.SetActive(false);// sets the game object to be inactive
    }
}
