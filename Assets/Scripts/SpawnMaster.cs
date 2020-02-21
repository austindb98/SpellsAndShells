using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System.Linq;

public class SpawnMaster : MonoBehaviour
{
    public bool isRescan = false;
    public List<EnemyController> enemyList;
    public List<GameObject> spawnPrefabList;    // the list of enemies to spawn
    public List<Vector3> spawnPositionList;
    public bool isRoomComplete = false;

    // Start is called before the first frame update
    void Start()
    {
        spawnEnemies();
    }

    // Update is called once per frame
    void Update()
    {
        if(isRescan) {
            AstarPath.active.Scan(AstarPath.active.data.gridGraph);
            isRescan = false;
        }
    }

    public void removeEnemyFromList(EnemyController enemy) {
        enemyList.Remove(enemy);
        if(enemyList.Count == 0) {
            isRoomComplete = true;
            // add in logic to open gate
        }
    }

    public void spawnEnemies() {
        GameObject enemyObj;

        foreach(EnemyEntry entry in spawnPrefabList.Zip(spawnPositionList, (prefab, pos) => new EnemyEntry(prefab, pos))) {
            enemyObj = Instantiate(entry.prefab, entry.position, Quaternion.Euler( 0f, 0f, 0f ));
            enemyObj.GetComponent<EnemyController>().spawnMaster = this;
            enemyList.Add(enemyObj.GetComponent<EnemyController>());
        }
    }
}

public class EnemyEntry {
    public GameObject prefab;
    public Vector3 position;

    public EnemyEntry(GameObject prefab, Vector3 position) {
        this.prefab = prefab;
        this.position = position;
    }
}
