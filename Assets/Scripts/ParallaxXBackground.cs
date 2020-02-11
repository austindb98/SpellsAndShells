using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParallaxXBackground : MonoBehaviour
{

    public float offsetScale = .05f;
    public bool invertedMovement = true;

    static readonly float hWidth = Screen.width / 2f;

    private float xPos;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
        Vector3 pos = Input.mousePosition;
        xPos = pos.x - hWidth;
        xPos *= offsetScale;
        if (invertedMovement)
        {
            xPos *= -1;
        }
        pos.x = hWidth + xPos;
        pos.y = transform.position.y;
        transform.position = pos;
    }
}
