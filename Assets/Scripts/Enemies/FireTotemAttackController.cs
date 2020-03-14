using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTotemAttackController : EnemyController
{
    private bool isSeeking = false;
    private float speed = 7f;
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        transform.localScale = new Vector3(10f, 10f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        if(isSeeking) {
            Vector3 towardsPlayer = player.transform.position - transform.position;
            towardsPlayer.Normalize();

            rb2d.velocity = speed * (new Vector2(towardsPlayer.x, towardsPlayer.y));
        }
    }

    public override void handleShotgunAttack(int damage) {
        isSeeking = false;
        rb2d.velocity = new Vector2(0f, 0f);
        an.SetBool("isExplode", true);
    }

    public override void handleAttack(float damage, BaseAttack.Element element) {
        isSeeking = false;
        rb2d.velocity = new Vector2(0f, 0f);
        an.SetBool("isExplode", true);
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        string tag = collider.gameObject.tag;
        if (tag != "Enemy" && tag != "Pickup" && tag != "Heart" && tag != "Potion" && tag != "BlueShell" && tag != "GreenShell" && tag != "Water") {
            isSeeking = false;
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
        isSeeking = true;
        GetComponent<Collider2D>().enabled = true;
    }
}
