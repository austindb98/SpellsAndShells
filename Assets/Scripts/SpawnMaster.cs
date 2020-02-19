using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class SpawnMaster : MonoBehaviour
{
    public bool isRescan = false;
    public List<EnemyController> enemyList;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isRescan) {
            print("rescanning");
            AstarPath.active.Scan(AstarPath.active.data.gridGraph);
            isRescan = false;
        }
    }
}
