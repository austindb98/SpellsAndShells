using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitchSpellController : MonoBehaviour
{
    public GameObject player;
    private Collider2D playerCollider;
    private PlayerController playerController;
    private Rigidbody2D rb2d;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        playerCollider = player.GetComponent<Collider2D>();
        playerController = player.GetComponent<PlayerController>();
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update() {
        Vector3 origVelocity = rb2d.velocity;
        Vector3 toPlayerVec = player.transform.position - transform.position;
        Vector3 newVelocity;

        origVelocity.Normalize();
        toPlayerVec.Normalize();
        newVelocity = 1f / Time.deltaTime * origVelocity + toPlayerVec;
        newVelocity.Normalize();
        rb2d.velocity = speed * newVelocity;

        float angle = Mathf.Atan2( rb2d.velocity.y, rb2d.velocity.x )  * Mathf.Rad2Deg + 90;
        transform.rotation = Quaternion.Euler( 0f, 0f, angle );
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        string tag = collider.gameObject.tag;
        if(tag != "Enemy" && tag != "Pickup" && tag != "Heart" && tag != "Potion" && tag != "BlueShell" && tag != "GreenShell" && tag != "Water")
        {
            if(collider == playerCollider) {
                playerController.takeDamage(10f);
                playerController.onHitKnockback(300.0f, transform.position);
            }
            Destroy(gameObject);
        }
    }
}
