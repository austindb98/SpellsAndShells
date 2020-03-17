using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public abstract class TargetedAttack : MonoBehaviour
{
    private SpriteRenderer target;
    protected GameObject toWatch;

    static readonly float distanceTriggerAt = .25f;

    protected virtual void Start()
    {
        target = GetComponent<SpriteRenderer>();
    }

    protected abstract void OnWatchedEnter();

    protected virtual void Update()
    {
        if (toWatch != null && Vector3.Distance(transform.position, toWatch.transform.position) < distanceTriggerAt)
        {
            target.enabled = false;
            OnWatchedEnter();
        }
    }
}
