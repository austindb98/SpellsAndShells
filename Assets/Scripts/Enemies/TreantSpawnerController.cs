using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreantSpawnerController : EnemyController
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        
    }

    public override void handleShotgunAttack() 
    {
        base.handleShotgunAttack();
    }

    public override void handleEnemyDeath() {
        base.handleEnemyDeath();
        base.spawnMaster.isRescan = true;
        Destroy(gameObject);
    }
}
