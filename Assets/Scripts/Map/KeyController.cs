using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.GetComponentInParent<PlayerController>();
        if (player)
        {
            GateController gate = transform.parent.GetComponentInChildren<GateController>();
            if (gate)
            {
                gate.keyFound = true;
            }
            Destroy(gameObject);
        }
    }
}
