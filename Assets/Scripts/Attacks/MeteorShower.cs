using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorShower : TargetedAttack
{
    public PhantomAttack phantomWatch;
    
    protected override void Start()
    {
        toWatch = phantomWatch.gameObject;
        base.Start();
    }

    protected override void OnWatchedEnter()
    {
        phantomWatch.Enable();
    }
}
