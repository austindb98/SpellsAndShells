using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextDamageController : MonoBehaviour
{
    private int lifetime = 2;
    private float deathTime;

    
    // Start is called before the first frame update
    void Start()
    {
        deathTime = Time.time + lifetime;
    }

    // Update is called once per frame
    void Update()
    {
        if(deathTime < Time.time)
        {
            Destroy(gameObject);
        }
    }
}
