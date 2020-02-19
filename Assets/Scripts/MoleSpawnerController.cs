using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoleSpawnerController : EnemyController
{
    public SpawnMaster spawnMaster;
    // Start is called before the first frame update
    public override void Start()
    {
        
    }

    // Update is called once per frame
    public override void Update()
    {
        
    }

    public override void handleShotgunHit(float knockbackMagnitude) { }

    public override void handleEnemyDeath() {
        spawnMaster.enemyList.Remove(this);
        spawnMaster.isRescan = true;
        Destroy(gameObject);
    }
}
