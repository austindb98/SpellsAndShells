using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MendohlSceneController : MonoBehaviour
{
    // assign these in prefab
    public GameObject portalPrefab;
    public GameObject skillPointPrefab;

    // assign these in scene
    public bool isGiveSkillPoint;
    public Vector3 portalSpawnLocation;
    public Vector3 skillPointSpawnLocation;
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
    void Update() {}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other == playerCollider)
        {
            startMendohlScene();
        }
    }

    public void startMendohlScene() {
        playerController.canMove = false;   // freeze player during dialogue
        Quaternion q = Quaternion.Euler( 0f, 0f, 0f );
        GameObject portal = Instantiate(portalPrefab, portalSpawnLocation, q);
        (portal.GetComponent<PortalController>()).sceneController = this;
    }

    public void handleMendohlSpawn() {
        foreach(string dialog in dialogList) {
            if(SaveManager.currentSave != null)
                DialogScheduler.addDialog(dialog.Replace("[Player Name]", SaveManager.currentSave.name));
            else
                DialogScheduler.addDialog(dialog);
        }
    }

    public void handlePortalClose() {
        playerController.canMove = true;
        if(isGiveSkillPoint)
            Instantiate(skillPointPrefab, skillPointSpawnLocation, new Quaternion(0, 0, 0, 0));
        Destroy(gameObject);
    }

}
