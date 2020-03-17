using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellScaler : MonoBehaviour
{
    static readonly float initialScale = 0;
    static readonly float finalScale = 1;

    static readonly float scaleSpeed = 3.2f;

    private float uniformScale;

    // Start is called before the first frame update
    void Start()
    {
        uniformScale = initialScale;
        transform.localScale = Vector3.one * initialScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (uniformScale < finalScale)
        {
            uniformScale += scaleSpeed * Time.deltaTime;
            if (uniformScale > finalScale)
            {
                uniformScale = finalScale;
            }
        }
        transform.localScale = Vector3.one * uniformScale;

    }
}
