using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    public GameObject player;
    private Collider2D playerCollider;
    private PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        playerCollider = player.GetComponent<Collider2D>();
        playerController = player.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        string tag = collider.gameObject.tag;
        if (tag != "Enemy" && tag != "Pickup" && tag != "Heart" && tag != "Potion" && tag != "BlueShell" && tag != "GreenShell" && tag != "Water") {
            if(collider == playerCollider)
                playerController.takeDamage(10f);
            Destroy(gameObject);
        }
    }
}
