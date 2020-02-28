using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomManager : MonoBehaviour
{

    public SpawnMaster spawnMaster;
    public GameObject roomDoors;
    public List<GameObject> doorList;
    public TilemapRenderer fog;

    private bool triggered;

    // Start is called before the first frame update
    void Start()
    {
        if (spawnMaster != null)
        {
            spawnMaster.gameObject.SetActive(false);
        }
        if (roomDoors != null)
        {
            roomDoors.SetActive(false);
        }
        if (fog != null)
        {
            fog.enabled = true;
        }
        triggered = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnMaster != null && roomDoors != null && triggered && spawnMaster.isRoomComplete)
        {
            roomDoors.SetActive(false);
        }
        else if (spawnMaster != null && roomDoors == null && spawnMaster.isRoomComplete)
        {
            foreach (GameObject door in doorList)
            {
                door.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            if (spawnMaster != null && fog.enabled)
            {
                spawnMaster.gameObject.SetActive(true);
                spawnMaster.spawnEnemies();
                if (roomDoors != null)
                {
                    roomDoors.SetActive(true);
                }
                else
                {
                    foreach (GameObject door in doorList)
                    {
                        door.SetActive(true);
                    }
                }
            }
            fog.enabled = false;
            triggered = true;
        }
    }

        


}
