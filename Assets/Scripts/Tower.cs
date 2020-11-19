using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower: MonoBehaviour
{
    private SpriteRenderer mySpriteRenderer;

    public void Select()
    {

        //shows and hides the range when tower is selected.
        mySpriteRenderer.enabled = !mySpriteRenderer.enabled;
    }


    // Start is called before the first frame update
    void Start()
    {
        mySpriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
