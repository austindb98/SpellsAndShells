using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDecorator : MonoBehaviour
{

    private float baseline;

    private static readonly float maxFromBaseline = .125f;

    private static readonly float lerpSpeed = .0135f;

    private bool down;

    // Start is called before the first frame update
    void Start()
    {
        baseline = transform.position.y;
    }

    private void MoveDelta(float direction)
    {
        transform.Translate(0, direction * lerpSpeed, 0); // deltatime causes flicker, not important to really use delta
    }

    // Update is called once per frame
    void Update()
    {
        if (down)
        {
            MoveDelta(-1);
            if (transform.position.y < baseline - maxFromBaseline)
            {
                down = false;
            }
        } else
        {
            MoveDelta(1);
            if (transform.position.y > baseline + maxFromBaseline)
            {
                down = true;
            }
        }
    }
}
