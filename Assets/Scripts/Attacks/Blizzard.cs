using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blizzard : TargetedAttack
{

    public new GameObject toWatch;


    protected override void Start()
    {
        base.toWatch = toWatch;
        base.Start();
    }

    protected override void OnWatchedEnter()
    {
        //do nothing
    }
}
