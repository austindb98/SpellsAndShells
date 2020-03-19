using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackNoRotation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.localRotation = Quaternion.identity;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
