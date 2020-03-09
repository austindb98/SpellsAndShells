using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MendohlSceneController : MonoBehaviour
{
    public Vector3 portalSpawnLocation;
    public GameObject portalPrefab;
    public DialogScheduler dialogScheduler;

    public List<string> dialogList;

    private PlayerController playerController;
    private Collider2D playerCollider;

    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        playerCollider = player.GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other == playerCollider)
        {
            playerController.canMove = false;   // freeze player during dialogue
            Quaternion q = Quaternion.Euler( 0f, 0f, 0f );
            GameObject portal = Instantiate(portalPrefab, portalSpawnLocation, q);
            (portal.GetComponent<PortalController>()).sceneController = this;
        }
    }

    public void handleMendohlSpawn() {
        foreach(string dialog in dialogList) {
            DialogScheduler.addDialog(dialog);
        }
    }

    public void handlePortalClose() {
        playerController.canMove = true;
        Destroy(gameObject);
    }

}
