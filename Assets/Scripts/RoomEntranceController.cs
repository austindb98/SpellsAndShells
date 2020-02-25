using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomEntranceController : MonoBehaviour
{
    private bool isEntranceTriggered = false;
    public Collider2D entranceBlocker;
    public Collider2D entranceTrigger;
    public SpawnMaster spawnMaster;

    // Start is called before the first frame update
    void Start()
    {
        //spawnMaster.doorList.Add(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        if(isEntranceTriggered) {
            return;
        }
        else {
            entranceBlocker.enabled = true;
            entranceTrigger.enabled = false;
            spawnMaster.spawnEnemies();
            spawnMaster.doorsVisible();
            Debug.Log("here");
            // clear fog of war? idk how that's done
        }
    }

}
