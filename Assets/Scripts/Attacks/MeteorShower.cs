using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorShower : MonoBehaviour
{
    public PhantomAttack toWatch;

    static readonly float distanceTriggerAt = .25f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, toWatch.transform.position) < distanceTriggerAt)
        {
            toWatch.Enable();
        }
    }
}
