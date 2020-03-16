using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateController : MonoBehaviour
{
    public bool keyFound;
    // Start is called before the first frame update
    void Start()
    {
        keyFound = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.GetComponentInParent<PlayerController>();
        if (player && keyFound)
        {
            Destroy(gameObject);
        }
    }
}
