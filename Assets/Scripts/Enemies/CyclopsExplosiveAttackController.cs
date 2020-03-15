using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CyclopsExplosiveAttackController : EnemyController
{
    private float speed = 10f;
    public float angle = 0f;

    public float stoppingTime = 0f; // if nonzero, attack stops after this much time
    private float stoppingTimer = 0f;
    private bool isStopped = false;

    public float relaunchTime = 0f;
    private float relaunchTimer = 0f;
    private bool isRelaunch = false;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        transform.localScale = new Vector3(10f, 10f, 1f);
    }

    // Update is called once per frame
    public override void Update()
    {
        if(!isStopped && stoppingTime > 0) {
            stoppingTimer += Time.deltaTime;
            if(stoppingTimer > stoppingTime) {
                rb2d.velocity = new Vector2(0, 0);
                isStopped = true;
            }
        }

        if(!isRelaunch && relaunchTime > 0) {
            relaunchTimer += Time.deltaTime;
            if(relaunchTimer > relaunchTime) {
                angle = 0f;
                handleLaunchExplosion();
                isRelaunch = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        if(collider.gameObject.tag != "Enemy" && collider.gameObject.tag != "DestructibleSpell" && collider.gameObject.layer != LayerMask.NameToLayer("Spells")) {
            rb2d.velocity = new Vector2(0f, 0f);
            an.SetBool("isExplode", true);
            if(collider == playerCollider)
                playerController.takeDamage(10f);
        }
    }

    public override void handleShotgunAttack(int damage) {
        rb2d.velocity = new Vector2(0f, 0f);
        an.SetBool("isExplode", true);
    }

    public override void handleAttack(float damage, BaseAttack.Element element) {
        rb2d.velocity = new Vector2(0f, 0f);
        an.SetBool("isExplode", true);
    }

    public void handleExplosionFinish() {
        Destroy(gameObject);
    }

    public void handleLaunchExplosion() {
        Vector2 velocity = new Vector2(player.transform.position.x - transform.position.x,
                                       player.transform.position.y - transform.position.y);
        velocity.Normalize();
        rb2d.velocity = Quaternion.Euler(0, 0, angle) * velocity * speed;
        an.SetBool("isLaunched", true);
        transform.localScale = new Vector3(1.5f, 1.5f, 1f);
    }
}
