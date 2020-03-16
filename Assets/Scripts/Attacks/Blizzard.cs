using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blizzard : TargetedAttack
{

    public GameObject toWatchPublic;

    private static readonly float lifetime = 2.5f;


    protected override void Start()
    {
        toWatch = toWatchPublic;
        Destroy(gameObject, lifetime);
        base.Start();
    }

    protected override void OnWatchedEnter()
    {
        //do nothing
    }
}
