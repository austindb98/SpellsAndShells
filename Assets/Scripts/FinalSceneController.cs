using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalSceneController : MonoBehaviour
{
    private GameObject player;
    private PlayerController playerController;
    public GameObject portalPrefab;
    public GameObject goldenShellPrefab;

    private Vector3 playerDestination;
    private FinalPortalController fpc;

    public GameObject skelWarrior;
    public GameObject skelArcher;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        playerController = player.GetComponent<PlayerController>();   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void handleSkeletonKingDown(Vector3 skelPos) {
        if(skelPos.x < 0f) {
            playerDestination = new Vector3(skelPos.x + 4f, skelPos.y, skelPos.z);
        }
        else {
            playerDestination = new Vector3(skelPos.x - 4f, skelPos.y, skelPos.z);
        }

        player.transform.position = playerDestination;

        Vector3 portalSpawnLocation;
        if(skelPos.y > 37f) {
            portalSpawnLocation = new Vector3(skelPos.x, skelPos.y - 4f, skelPos.z);
        }
        else {
            portalSpawnLocation = new Vector3(skelPos.x, skelPos.y + 4f, skelPos.z);
        }

        playerController.canMove = false;   // freeze player during dialogue
        Quaternion q = Quaternion.Euler( 0f, 0f, 0f );
        GameObject portal = Instantiate(portalPrefab, portalSpawnLocation, q);
        fpc = portal.GetComponent<FinalPortalController>();
        fpc.sceneController = this;
    }

    public void handleShootSkeletonKing() {
        DialogScheduler.addDialog("Muahahahahaha", true);
        DialogScheduler.addDialog("The world shall finally meet its dark end.", true);
        DialogScheduler.addDialog("So long, [Player Name]. Do try to enjoy your final moments. You've worked hard for this.", true);
        fpc.isDisappear = true;
        fpc.mendohl.GetComponent<Collider2D>().enabled = false;
    }

    public void handleShootMendohl() {
        Destroy(fpc.mendohl);
        DialogScheduler.addDialog("You have done a brave thing, [Player Name].", false);
        DialogScheduler.addDialog("It takes a wise and courageous soul to stand up to a being of his power.", false);
        DialogScheduler.addDialog("Thanks to you, the underworld is finally safe from that two-faced trickster.", false);
        DialogScheduler.addDialog("And I think I know exactly how to repay you.", false);
        DialogScheduler.addDialog("[Player Name], I think it's about time you go back to your own world.", false);
    }

    public void handleFinishDialog() {
        Instantiate(goldenShellPrefab, new Vector3(0f, 30f, 0f), Quaternion.Euler( 0f, 0f, 0f ));
        playerController.canMove = true;
    }

    public void handleMendohlSpawn() {
        DialogScheduler.addDialog("Malphos is weakened, but a being of his power can't be killed so easily..", true);
        DialogScheduler.addDialog("NO! PLEASE!", false);
        DialogScheduler.addDialog("You have no idea what you're doing.", false);
        DialogScheduler.addDialog("I am the keeper of the underworld. I am the only one who can contain the creatures that lurk here.", false);
        DialogScheduler.addDialog("If you slay me, unknown evils will be unleashed on the world.", false);
        DialogScheduler.addDialog("LIES. You must not listen to him. Malphos protects the evil in this world, and he must be slain.", true);
        DialogScheduler.addDialog("Take this golden shell. It has special properties and has the power to kill gods.", true);
        DialogScheduler.addDialog("End this, [Player Name]. Once and for all.", true);
        DialogScheduler.addDialog("Dewit.", true);
    }

    public void handlePortalClose() {
        playerController.canMove = true;
    }

    public void SpawnSkeletons() {
        float[] xPosAr = {-10, -6, -2, 2, 6, 10};
        float[] yPosAr = {33, 35.5f, 38, 40.5f, 43f};
        int counter = 0;

        foreach(float x in xPosAr) {
            foreach(float y in yPosAr) {
                if(CheckCanSpawn(x, y)) {
                    if(counter % 4 > 0)
                        Instantiate(skelWarrior, new Vector3(x, y, 0f), new Quaternion(0, 0, 0, 0));
                    else
                        Instantiate(skelArcher, new Vector3(x, y, 0f), new Quaternion(0, 0, 0, 0));
                    counter++;
                }
            }
        }
    }

    private bool CheckCanSpawn(float x, float y) {
        Vector3 pos = new Vector3(x, y, 0f);
        bool canSpawn = true;
        if((pos - player.transform.position).magnitude < 2f)
            canSpawn = false;
        return canSpawn;
    }
}
