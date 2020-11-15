using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hover :Singleton<Hover>
{

    private SpriteRenderer spriteRenderer;

    private void FollowMouse()
    {
        if (spriteRenderer.enabled) //Hover should only follow the mouse/drag when hover is activated.
        {
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        }
        
    }


    public void Activate(Sprite sprite)
    {
        if (spriteRenderer.enabled !=true)
        { spriteRenderer.enabled = true; }
        this.spriteRenderer.sprite = sprite;
    }

    public void Deactivate()
    {
        spriteRenderer.enabled = false;
        GameManager.Instance.ClickedButton = null;
    }

    // Start is called before the first frame update
    void Start()
    {
        this.spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        FollowMouse();
    }
}
