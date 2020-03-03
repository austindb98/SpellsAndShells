using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootprintController : MonoBehaviour
{
    private float creationTime;
    private static float lifeTime = 1;
    private SpriteRenderer renderer;
    private static float baseTransparency = 0.5f;
    
    // Start is called before the first frame update
    void Start()
    {
        creationTime = Time.time;
        renderer = transform.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time > creationTime + lifeTime)
        {
            Destroy(gameObject);
        }
        else
        {
            Color color = renderer.color;
            float transparency = ((lifeTime - (Time.time - creationTime)) / lifeTime) * baseTransparency;
            color.a = transparency;
            renderer.color = color;

        }
    }
}
