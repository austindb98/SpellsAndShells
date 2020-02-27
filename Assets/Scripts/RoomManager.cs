using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomManager : MonoBehaviour
{

    public SpawnMaster spawnMaster;
    public GameObject roomDoors;
    public List<GameObject> doorList;

    private TilemapRenderer fog;
    private TilemapCollider2D trigger;

    // Start is called before the first frame update
    void Start()
    {
        fog = this.gameObject.GetComponent<TilemapRenderer>();
        trigger = this.gameObject.GetComponent<TilemapCollider2D>();
        if (roomDoors != null)
        {
            roomDoors.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnMaster != null)
        {
            Debug.Log(spawnMaster.isRoomComplete);
        }
        if (spawnMaster != null && roomDoors != null && spawnMaster.isRoomComplete)
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
        }
    }

        


}
