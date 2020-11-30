﻿using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    [SerializeField]
    private float cameraSpeed;

    [SerializeField]
    private float mobileCameraSpeed;

    private float xMax;
    private float yMin;

    /// <summary>
    /// sets the boundaries that the camera can move in.
    /// </summary>
    /// <param name="maxTile"></param>
    public void SetLimits(Vector3 maxTile)
    {
        Vector3 wp = Camera.main.ViewportToWorldPoint(new Vector3(1, 0));

        xMax = maxTile.x - wp.x;
        yMin = maxTile.y - wp.y;
    }



    private void GetInput()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.up * cameraSpeed * Time.deltaTime);

        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.down * cameraSpeed * Time.deltaTime);

        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.left * cameraSpeed * Time.deltaTime);

        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * cameraSpeed * Time.deltaTime);

        }


        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved && GameManager.Instance.ClickedButton==null)
        {
            Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
            transform.Translate(-touchDeltaPosition.x * cameraSpeed * Time.deltaTime, -touchDeltaPosition.y * cameraSpeed * Time.deltaTime, 0);
        }




        // prevents the camera from moving beyond the set boundaries.
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, 0, xMax), Mathf.Clamp(transform.position.y, yMin, 0), -10); 
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        GetInput();
    }
}
