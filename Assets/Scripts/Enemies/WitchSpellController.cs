using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitchSpellController : MonoBehaviour
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
        if(collider.gameObject.tag != "Enemy") {
            if(collider == playerCollider) {
                playerController.takeDamage(10f);
                playerController.onHitKnockback(1000.0f, transform.position);
            }
            Destroy(gameObject);
        }
    }
}
