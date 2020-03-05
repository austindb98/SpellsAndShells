﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Pathfinding;

public class MinotaurBossController : EnemyController
{
    public enum MinotaurState {
        Beyblade, Normal, Charge, Stunned
    }

    private MinotaurState minotaurState;
    private bool isSwingRest = false;
    private float swingRestTime = 1.5f;
    private float swingRestTimer = 0f;

    private float deathTimer = 0f;
    private float deathTime = 1.2f;
    private bool isDead = false;

    private float lungeTime = 1f;
    private float lungeTimer = 0f;
    private bool isLunging = false;

    private float beybladeStartTime = 12f;
    private float beybladeFinishTime = 24f;
    private float beybladeTimer = 0f;
    private bool isBeyblade = false;

    private bool isCompletelyDead = false;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        minotaurState = MinotaurState.Normal;
        knockbackTime = 0.2f;
        knockbackCoefficient = 0.05f;
        attackStrength = 25;
    }

    // Update is called once per frame
    public override void Update()
    {
        float x = player.transform.position.x - transform.position.x;
        if(isCompletelyDead)
            return;
        
        if(isDead) {
            deathTimer += Time.deltaTime;
            if(deathTimer > deathTime) {
                base.handleEnemyDeath();
                isCompletelyDead = true;
            }
        }

        beybladeTimer += Time.deltaTime;
        if(!isBeyblade) {
            print("2");
            if(beybladeTimer > beybladeStartTime && !isDead) {
                BeybladeAttack();
                return;
            }
        }
        else {
            if(beybladeTimer > beybladeFinishTime) {
                beybladeTimer = 0f;
                isBeyblade = false;
                an.SetBool("isAttack2", false);
                aiPath.canMove = true;
                rb2d.sharedMaterial.bounciness = 0;
                CapsuleCollider2D[] colliders = GetComponents<CapsuleCollider2D>();
                colliders[0].sharedMaterial.bounciness = 0;
                colliders[1].sharedMaterial.bounciness = 0;
                rb2d.drag = 4;
            }
            else {
                if(rb2d.velocity.magnitude < 25f) {
                    rb2d.velocity = new Vector2(-25f, -12f);
                }
            }
            return;
        }

        base.Update();

        if(isSwingRest) {
            aiPath.canMove = false;
            swingRestTimer += Time.deltaTime;
            if(swingRestTimer > swingRestTime) {
                isSwingRest = false;
                swingRestTimer = 0f;
                an.SetBool("isWalking", true);
            }
        }

        if(isLunging) {
            lungeTimer += Time.deltaTime;
            if(lungeTimer > lungeTime) {
                isLunging = false;
                lungeTimer = 0f;
                aiPath.canMove = true;
            }
        }

        if(!base.isKnockback && !isSwingRest && !isDead) {
            aiPath.canMove = true;

            if(!isLunging && Vector3.Distance(player.transform.position, transform.position) < 8f) {
                aiPath.canMove = false;
                Vector2 playerVec = new Vector2(player.transform.position.x, player.transform.position.y);
                Vector2 minotaurVec = new Vector2(transform.position.x, transform.position.y);

                rb2d.velocity = 15f * (playerVec - minotaurVec).normalized;
            }
        }
        else
            aiPath.canMove = false;

        if (!isDead && minotaurState == MinotaurState.Normal) {
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
    }

    public void BeybladeAttack() {
        an.SetBool("isAttack2", true);
        aiPath.canMove = false;
        rb2d.velocity = new Vector2(25f, 12f);
        CapsuleCollider2D[] colliders = GetComponents<CapsuleCollider2D>();
        colliders[0].sharedMaterial.bounciness = 1;
        colliders[1].sharedMaterial.bounciness = 1;
        rb2d.sharedMaterial.bounciness = 1;
        rb2d.drag = 0;
        isBeyblade = true;
        isKnockback = false;
    }

    private void OnTriggerStay2D(Collider2D other) {
        if(other == playerCollider && (!isKnockback || isBeyblade)) {
            if(isBeyblade) {
                playerController.takeDamage(3f);
                playerController.onHitKnockback(800.0f, transform.position);
            }
            else if(!isSwingRest)
                an.SetBool("isAttack1", true);
        }
    }
        
    private void OnTriggerEnter2D(Collider2D other) {
        if(other == playerCollider && (!isKnockback || isBeyblade)) {
            if(isBeyblade) {
                playerController.takeDamage(5f);
                playerController.onHitKnockback(800.0f, transform.position);
            }
            else if(!isSwingRest)
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

    public override void applyKnockback(float knockbackMagnitude) {
        if(!isBeyblade)
            base.applyKnockback(knockbackMagnitude);
    }

    public override void handleEnemyDeath() {
        if (isDead)
            return; // so animation doesn't keep on playing

        Collider2D[] colliderAr = gameObject.GetComponents<Collider2D>();
        colliderAr[0].enabled = false;
        colliderAr[1].enabled = false;
        aiPath.canMove = false;
        an.SetBool("isDead", true);
        isDead = true;
        isBeyblade = false;
        an.SetBool("isAttack2", false);
        rb2d.velocity = new Vector2(0f, 0f);
    }

    private void WalkLeft()
    {
        an.SetBool("isFacingRight", false);
        an.SetBool("isWalking", true);
    }

    private void WalkRight()
    {
        an.SetBool("isFacingRight", true);
        an.SetBool("isWalking", true);
    }

    public void handleAttack() {
        if (Vector3.Distance(transform.position, player.transform.position) < 4.0f)
        {
            playerController.takeDamage(attackStrength);
            playerController.onHitKnockback(1500.0f, transform.position);
        }
        isSwingRest = true;
        swingRestTimer = 0f;
        aiPath.canMove = false;
        an.SetBool("isAttack1", false);
        an.SetBool("isWalking", false);
    }
}
