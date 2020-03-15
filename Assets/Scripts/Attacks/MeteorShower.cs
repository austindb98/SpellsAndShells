using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (SpriteRenderer))]
public class MeteorShower : MonoBehaviour
{
    public PhantomAttack toWatch;

    private SpriteRenderer sr;

    static readonly float distanceTriggerAt = .25f;
    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (toWatch != null && Vector3.Distance(transform.position, toWatch.transform.position) < distanceTriggerAt)
        {
            toWatch.Enable();
            sr.enabled = false;
        }
    }
}
