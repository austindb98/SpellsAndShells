using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackNoRotation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.rotation = Quaternion.AngleAxis(0, Vector3.forward);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
