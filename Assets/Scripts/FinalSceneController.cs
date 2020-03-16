using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalSceneController : MonoBehaviour
{
    private enum FinalSceneState {
        SkelFight, MovePlayer, Dialog, PlayerChoice, KillSkel, KillGod
    }

    private GameObject player;
    private PlayerController playerController;
    public GameObject portalPrefab;
    public GameObject goldenShellPrefab;

    private FinalSceneState state;
    private Vector3 playerDestination;

    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        playerController = player.GetComponent<PlayerController>();   
        state = FinalSceneState.SkelFight;
    }

    // Update is called once per frame
    void Update()
    {
        /*switch(state) {
            case FinalSceneSkelFight:
                break;
            case MovePlayer:
                break;
            case Dialog:
                break;
            case PlayerChoice:
                break;
            case KillSkel:
                break;
            case KillGod:
                break;
                
        }*/
        
    }

    public void handleSkeletonKingDown(Vector3 skelPos) {
        if(skelPos.x < 0f) {
            playerDestination = new Vector3(skelPos.x + 3f, skelPos.y, skelPos.z);
        }
        else {
            playerDestination = new Vector3(skelPos.x - 3f, skelPos.y, skelPos.z);
        }
        player.transform.position = playerDestination;

        Vector3 portalSpawnLocation;
        if(skelPos.y > 33f) {
            portalSpawnLocation = new Vector3(skelPos.x, skelPos.y - 3f, skelPos.z);
        }
        else {
            portalSpawnLocation = new Vector3(skelPos.x, skelPos.y + 3f, skelPos.z);
        }

        playerController.canMove = false;   // freeze player during dialogue
        Quaternion q = Quaternion.Euler( 0f, 0f, 0f );
        GameObject portal = Instantiate(portalPrefab, portalSpawnLocation, q);
        (portal.GetComponent<FinalPortalController>()).sceneController = this;
    }

    public void handleFinishDialog() {
        Instantiate(goldenShellPrefab, new Vector3(0f, 30f, 0f), Quaternion.Euler( 0f, 0f, 0f ));
        playerController.canMove = true;
    }

    public void handleMendohlSpawn() {
        DialogScheduler.addDialog("meep1", true);
        DialogScheduler.addDialog("meep2", false);
        DialogScheduler.addDialog("meep3", true);
        DialogScheduler.addDialog("meep4", false);
    }

    public void handlePortalClose() {
        playerController.canMove = true;
    }
}
