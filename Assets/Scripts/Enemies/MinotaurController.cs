﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Pathfinding;

public class MinotaurController : EnemyController
{
    private bool isSwingRest = false;
    private float swingRestTime = 1f;
    private float swingRestTimer = 0f;

    private float deathTimer = 0f;
    private float deathTime = 1.2f;
    private bool isDead = false;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        knockbackCoefficient = 0.5f;
        attackStrength = 25;
    }

    // Update is called once per frame
    public override void Update()
    {        
        float x = aiPath.desiredVelocity.x;
        float y = aiPath.desiredVelocity.y;

        base.Update();

        if(isSwingRest) {
            swingRestTimer += Time.deltaTime;
            if(swingRestTimer > swingRestTime) {
                isSwingRest = false;
                swingRestTimer = 0f;
            }
        }
        
        if(!base.isKnockback && !isSwingRest && !isDead)
            aiPath.canMove = true;
        else
            aiPath.canMove = false;

        if(base.isKnockback || isSwingRest) {
            return;
        }
        else if (x == 0 && y == 0)
        {
            an.SetBool("isWalking", false);
        }
        else if (x > 0)
            WalkRight();
        else if (x < 0)
            WalkLeft();
        else
            an.SetBool("isWalking", true);
        

        if(isDead) {
            deathTimer += Time.deltaTime;
            if(deathTimer > deathTime)
            {
                base.handleEnemyDeath();
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other == playerCollider && !isKnockback) {
            if(!isSwingRest)
                an.SetBool("isAttack1", true);
        }
    }

    public override void handleShotgunHit(float knockbackStrength) {
        base.handleShotgunHit(knockbackStrength * knockbackCoefficient);
        
        base.isKnockback = true;
        base.aiPath.canMove = false;
        an.SetBool("isWalking", false);
        an.SetBool("isAttack1", false);
    }

    public override void handleEnemyDeath() {
        if (isDead)
            return; // so animation doesn't keep on playing

        aiPath.canMove = false;
        an.SetBool("isDead", true);
        isDead = true;
        base.handleEnemyDeath();
        //Destroy(gameObject);
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
    }
}