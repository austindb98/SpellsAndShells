﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Pathfinding;

public class EnemyTreantGraphics : EnemyController
{

    public void Start()
    {
        base.Start();
        base.knockbackCoefficient = 1f;
    }

    // Update is called once per frame
    public void Update()
    {
        float x = aiPath.desiredVelocity.x;
        float y = aiPath.desiredVelocity.y;

        base.Update();
        
        if(isKnockback) {
            return;
        }
        else if(x == 0 && y == 0) {
            an.SetBool("isTreantWalking", false);
        }
        else if(x >= Math.Abs(y))
            WalkRight();
        else if(x <= -1 * Math.Abs(y))
            WalkLeft();
        else if(y >= Math.Abs(x))
            WalkUp();
        else if(y <= -1 * Math.Abs(x))
            WalkDown();
        else {
            print("animation oof");
        }

    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other == playerCollider && !isKnockback) {
            handleShotgunHit(1f);
            playerController.takeDamage(attackStrength);
        }
    }

    public override void handleShotgunHit(float knockbackStrength) {
        base.handleShotgunHit(knockbackStrength * knockbackCoefficient);
        
        base.isKnockback = true;
        base.aiPath.canMove = false;
        print("can't move");
        base.an.SetBool("isTreantWalking", false);
    }

    public override void handleEnemyDeath() {
        Destroy(gameObject);
    }

    private void WalkLeft() {
        an.SetInteger("treantDirection", 4);
        an.SetBool("isTreantWalking", true);
    }

    private void WalkUp() {
        an.SetInteger("treantDirection", 3);
        an.SetBool("isTreantWalking", true);
    }

    private void WalkRight() {
        an.SetInteger("treantDirection", 2);
        an.SetBool("isTreantWalking", true);
    }

    private void WalkDown() {
        an.SetInteger("treantDirection", 1);
        an.SetBool("isTreantWalking", true);
    }
}
