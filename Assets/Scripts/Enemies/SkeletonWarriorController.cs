using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Pathfinding;

public class SkeletonWarriorController : EnemyController
{
    private bool isSwingRest = false;
    private float swingRestTime = 1f;
    private float swingRestTimer = 0f;

    private float reviveTimer = 0f;
    private float reviveTime = 5f;
    private bool isDead = false;
    private bool isReviving = false;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        knockbackStrength = 0.1f;
        attackStrength = 35;
        knockbackTime = 0.4f;
    }

    // Update is called once per frame
    public override void Update()
    {
        float x = player.transform.position.x - transform.position.x;
        base.Update();

        if(isSwingRest) {
            swingRestTimer += Time.deltaTime;
            if(swingRestTimer > swingRestTime) {
                isSwingRest = false;
                swingRestTimer = 0f;
            }
        }

        if(!base.isKnockback && !isSwingRest && !isDead && !isReviving)
            aiPath.canMove = true;
        else
            aiPath.canMove = false;
        if (!isDead)
        {
            if (base.isKnockback || isSwingRest)
                return;
            else if (aiPath.desiredVelocity.x == 0 && aiPath.desiredVelocity.y == 0)
                an.SetBool("isWalking", false);
            else if (x > 0)
                WalkRight();
            else if (x < 0)
                WalkLeft();
            else
                an.SetBool("isWalking", true);
        }


        if(isDead) {
            reviveTimer += Time.deltaTime;
            if(reviveTimer > reviveTime) {
                StartRevival();
            }
        }
    }

    public void FinishRevival() {
        aiPath.canMove = false;
        isReviving = false;
        rb2d.constraints &= ~RigidbodyConstraints2D.FreezePosition;

    }

    private void StartRevival() {
        isReviving = true;  
        isDead = false;
        an.SetBool("isDead", false);
        enemyHealth.setCurrentHealth(150f);
        aiPath.canMove = false;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other == playerCollider && !isKnockback) {
            if(!isSwingRest)
                an.SetBool("isAttack1", true);
        }
    }

    public override void handleShotgunAttack(int dmg) {
        base.handleShotgunAttack(dmg);

        base.isKnockback = true;
        base.aiPath.canMove = false;
        an.SetBool("isWalking", false);
        an.SetBool("isAttack1", false);
    }

    public override void handleEnemyDeath() {
        if (isDead) {
            base.handleEnemyDeath();
            Destroy(gameObject);
            return;
        }
        aiPath.canMove = false;
        an.SetBool("isDead", true);
        reviveTimer = 0f;
        isDead = true;

        rb2d.constraints |= RigidbodyConstraints2D.FreezePosition;
        enemyHealth.setCurrentHealth(80f);
    }

    private void WalkLeft() {
        an.SetBool("isFacingRight", false);
        an.SetBool("isWalking", true);
    }

    private void WalkRight() {
        an.SetBool("isFacingRight", true);
        an.SetBool("isWalking", true);
    }

    public void handleAttack() {
        if (Vector3.Distance(transform.position, player.transform.position) < 4.0f)
        {
            playerController.takeDamage(attackStrength);
            playerController.onHitKnockback(500.0f, transform.position);
        }
        isSwingRest = true;
        swingRestTimer = 0f;
        aiPath.canMove = false;
        an.SetBool("isAttack1", false);
    }
}
