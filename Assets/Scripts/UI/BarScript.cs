using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarScript : MonoBehaviour
{
    private float maxValue;
    public float MaxValue
    {
        get
        {
            return maxValue;
        }
        set
        {
            maxValue = value;
        }
    }

    private float currentValue;

    public float CurrentValue
    {
        get
        {
            return currentValue;
        }
        set
        {
            currentValue = value;
        }
    }

    //public float currentValue;

    private Sprite barFill;

    private float originalScale;


    public void UpdateBar()
    {
        Vector3 tmpScale = gameObject.transform.localScale;
        tmpScale.x = currentValue / maxValue * originalScale;
        gameObject.transform.localScale = tmpScale;
    }

    // Start is called before the first frame update
    void Start()
    {
        originalScale = gameObject.transform.localScale.x;

    }

    // Update is called once per frame
    void Update()
    {
        UpdateBar();

    }
}
