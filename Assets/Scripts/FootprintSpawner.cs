using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FootprintSpawner : MonoBehaviour
{
    private Vector3 lastPosition;
    private float lastTime;
    private static float deltaTime = 1f;
    public GameObject footprintPrefab;
    private Vector2 mousePos;
    private static readonly float temporalSpacing = 1.5f;
    private static readonly float yOffset = 1.1f;

    // Start is called before the first frame update
    void Start()
    {
        CreateFootprint();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, lastPosition) > 1 || (Time.time - lastTime) > temporalSpacing)
        {
            CreateFootprint();
        }
    }
    private float getMouseAngle()
    {
        return Mathf.Atan2(mousePos.y - transform.position.y,
            mousePos.x - transform.position.x);
    }
    
    private float getFootprintAngle()
    {
        float angle = (float)(getMouseAngle() / Math.PI * 180);
        Vector2 position = new Vector2(transform.position.x, transform.position.y);
        Vector2 diff = (mousePos - position);

        if ((angle >= -22.5 && angle < 22.5) && diff.x > 0)
        {
            //frontRight;
            angle = -90;
        }
        else if ((angle >= 22.5 && angle < 67.5) && diff.x > 0)
        {
            //diagUpRight;
            angle = -45;
        }
        else if ((angle >= 67.5 && angle < 112.5) && diff.y > 0)
        {
            //spriteRenderer.sprite = back;
            angle = 0;

        }
        else if ((angle >= 112.5 && angle < 157.5) && diff.y > 0)
        {
            //diagUpLeft;
            angle = 45;

        }
        else if ((angle >= 157.5 || angle < -157.5) && diff.x < 0)
        {
            //frontLeft;
            angle = 90;

        }
        else if ((angle >= -157.5 && angle < -112.5) && diff.x < 0)
        {
            //diagDownLeft;
            angle = 135;

        }
        else if ((angle >= -112.5 && angle < -67.5) && diff.y < 0)
        {
            //frontDown;
            angle = 180;
        }
        else if ((angle >= -67.5 && angle < -22.5) && diff.x > 0)
        {
            //diagDownRight;
            angle = -135;
        }

        return angle;
    }
    

    void CreateFootprint()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        lastTime = Time.time;
        lastPosition = transform.position;
        GameObject footprint = Instantiate(footprintPrefab);
        Vector3 placement = transform.position;
        placement.y = placement.y - yOffset;
        Quaternion rot = Quaternion.Euler(0, 0, getFootprintAngle());
        footprint.transform.SetPositionAndRotation(placement, rot);
    }
}
