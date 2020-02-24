using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Pathfinding;

public class WitchController : EnemyController
{
    private float maxHexRange = 25f;     // range from which archer can attack
    private float shotPrepTime = 1f;        // time needed for shot cooldown
    private float shotPrepTimer;            // cooldown timer for shooting arrow
    private bool isPreppingShot = false;    // indicates if shot is being prepped
    private int raycastLayerMask;

    private float deathTimer;
    private float deathTime = 1.2f;
    private bool isDead;

    public GameObject spell;

    public override void Start()
    {
        base.Start();
        raycastLayerMask =  ((1 << LayerMask.NameToLayer("Obstacles")) |
                             (1 << LayerMask.NameToLayer("Walls")) |
                             (1 << LayerMask.NameToLayer("Player")));
    }

    // Update is called once per frame
    public override void Update()
    {
        float x = player.transform.position.x - transform.position.x;
        base.Update();

        if(isDead) {
            deathTimer += Time.deltaTime;
            if(deathTimer > deathTime) {
                base.handleEnemyDeath();
                Destroy(gameObject);
            }
        }
        else if(isKnockback) {
            return;
        }
        else if(CheckLineOfSight()) {    // target is in LoS. prep shot
            aiPath.canMove = false;
            an.SetBool("isWalking", false);
            an.SetBool("isHexing", true);
            an.SetBool("isFacingRight", x > 0);
            print(x > 0);
        }
        else if(aiPath.desiredVelocity.x == 0 && aiPath.desiredVelocity.y == 0) { // idle
            an.SetBool("isWalking", false);
        }
        else if(x > 0) {
            an.SetBool("isFacingRight", true);
            an.SetBool("isWalking", true);
        }
        else if(x < 0) {
            an.SetBool("isFacingRight", false);
            an.SetBool("isWalking", true);
        }
        else {
            an.SetBool("isWalking", true);
        }
    }

    // shoots an arrow at where the player currently is. I think we should have a variation to this where
    // the shot is fired at where the player will be when the arrow arrives
    private void ShootArrow() {
        Vector3 dir = player.transform.position - transform.position;
        float angle = Mathf.Atan2( dir.y, dir.x )  * Mathf.Rad2Deg + 90;
        Quaternion q = Quaternion.Euler( 0f, 0f, angle );
        Vector3 unitVec = player.transform.position - transform.position;
        unitVec.Normalize();

        GameObject thisArrow = Instantiate(spell, transform.position, q);
        thisArrow.GetComponent<Rigidbody2D>().velocity = 30f * unitVec;
        thisArrow.GetComponent<ArrowController>().player = player;
        an.SetBool("isHexing", false);
        base.aiPath.canMove = true;
    }

    // returns whether the player is in LoS of the ArcherBoy
    private bool CheckLineOfSight() {
        if(Vector3.Distance(player.transform.position, transform.position) < maxHexRange) {
            Vector3 rayDirection = player.transform.position - transform.position;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDirection, maxHexRange, raycastLayerMask);

            if (hit)
                return hit.transform == player.transform;
        }
        return false;
    }

    public override void handleShotgunHit(float knockbackMagnitude) {
        Vector2 unitVec = transform.position - player.transform.position;
        unitVec.Normalize();
        rb2d.AddForce(unitVec * knockbackMagnitude);

        base.isKnockback = true;
        base.aiPath.canMove = false;
        an.SetBool("isWalking", false);
    }

    public void handleHex() {
        // hex was just cast
        ShootArrow();
    }

    public override void handleEnemyDeath() {
        if (isDead)
            return; // so animation doesn't keep on playing

        Collider2D[] colliderAr = gameObject.GetComponents<Collider2D>();
        colliderAr[0].enabled = false;
        colliderAr[1].enabled = false;
        aiPath.canMove = false;
        an.SetBool("isDying", true);
        isDead = true;
        base.handleEnemyDeath();
    }
}
