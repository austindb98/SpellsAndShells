using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CyclopsExplosiveAttackController : EnemyController
{
    private float speed = 10f;
    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = new Vector3(10f, 10f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        if(collider.gameObject.tag != "Enemy") {
            rb2d.velocity = new Vector2(0f, 0f);
            an.SetBool("isExplode", true);
            if(collider == playerCollider)
                playerController.takeDamage(10f);
        }
    }

    public void handleExplosionFinish() {
        Destroy(gameObject);
    }

    public void handleLaunchExplosion() {
        Vector2 velocity = new Vector2(player.transform.position.x - transform.position.x,
                                       player.transform.position.y - transform.position.y);
        velocity.Normalize();
        rb2d.velocity = speed * velocity;
        an.SetBool("isLaunched", true);
        transform.localScale = new Vector3(1.5f, 1.5f, 1f);
    }
}
