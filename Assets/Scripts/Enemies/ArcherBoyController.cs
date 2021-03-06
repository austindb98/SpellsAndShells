﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Pathfinding;

public class ArcherBoyController : EnemyController
{
    private float maxArcherRange = 15f;     // range from which archer can attack
    private float shotAnimationTime = 0.3f; // this is 100% guesswork. How to actually check this value?
    private float shotAnimationTimer;       // timer for shot animation sequence
    private bool isFireShot = false;        // indicates if in shot animation sequences
    private float shotPrepTime = 1f;        // time needed for shot cooldown
    private float shotPrepTimer;            // cooldown timer for shooting arrow
    private bool isPreppingShot = false;    // indicates if shot is being prepped
    private int raycastLayerMask;

    public GameObject arrow;
    private System.Random rnd;

    public override void Start()
    {
        base.Start();
        rnd = new System.Random();
        raycastLayerMask =  ((1 << LayerMask.NameToLayer("Obstacles")) |
                             (1 << LayerMask.NameToLayer("Walls")) |
                             (1 << LayerMask.NameToLayer("Player")));
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        if(isKnockback) {
            return;
        }
        else if(isFireShot) {    // increment timer only if in animation sequences
            SetOrientation(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y);
            shotAnimationTimer += Time.deltaTime;
            // check if shot animation sequence has completed and arrow can be fired
            if(shotAnimationTimer > shotAnimationTime) {
                isFireShot = false;     // no longer in shot animation sequence
                ShootArrow();
                an.SetBool("isShooting", false);
                aiPath.canMove = true;
            }
        }
        else if(isPreppingShot) {
            SetOrientation(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y);
            shotPrepTimer += Time.deltaTime;
            if(shotPrepTimer > shotPrepTime) {
                isFireShot = true;
                isPreppingShot = false;
                shotAnimationTimer = 0f;
                an.SetBool("isShooting", true);
            }
        }
        else if(CheckLineOfSight()) {    // target is in LoS. prep shot
            isPreppingShot = true;
            aiPath.canMove = false;
            an.SetBool("isWalking", false);
            shotPrepTimer = 0f;
            shotPrepTime = (float) rnd.NextDouble() / 2f + .5f; // get random val between .5 and 1
        }
        else if(aiPath.desiredVelocity.x == 0 && aiPath.desiredVelocity.y == 0) { // idle
            an.SetBool("isWalking", false);
        }
        else {
            an.SetBool("isWalking", true);
            SetOrientation(aiPath.desiredVelocity.x, aiPath.desiredVelocity.y);
        }
    }

    private void SetOrientation(float x, float y) {
        if(x >= Math.Abs(y))
            an.SetInteger("direction", 2);
        else if(x <= -1 * Math.Abs(y))
            an.SetInteger("direction", 4);
        else if(y >= Math.Abs(x))
            an.SetInteger("direction", 3);
        else
            an.SetInteger("direction", 1);
    }

    // shoots an arrow at where the player currently is. I think we should have a variation to this where
    // the shot is fired at where the player will be when the arrow arrives
    private void ShootArrow() {
        Vector3 dir = player.transform.position - transform.position;
        float angle = Mathf.Atan2( dir.y, dir.x )  * Mathf.Rad2Deg - 90;
        Quaternion q = Quaternion.Euler( 0f, 0f, angle );
        Vector3 unitVec = player.transform.position - transform.position;
        unitVec.Normalize();

        GameObject thisArrow = Instantiate(arrow, transform.position, q);
        thisArrow.GetComponent<Rigidbody2D>().velocity = 30f * unitVec;
        thisArrow.GetComponent<ArrowController>().player = player;
    }

    // returns whether the player is in LoS of the ArcherBoy
    private bool CheckLineOfSight() {
        bool isAllHit = true;

        if(Vector3.Distance(player.transform.position, transform.position) < maxArcherRange) {
            Vector3[] rayStartingPoints = {
                transform.position + new Vector3(0.5f, 0, 0),
                transform.position + new Vector3(-0.5f, 0, 0),
                transform.position + new Vector3(0, 0.5f, 0),
                transform.position + new Vector3(0, -0.5f, 0)
            };
            foreach(Vector3 initialPos in rayStartingPoints) {
                Vector3 rayDirection = player.transform.position - initialPos;
                RaycastHit2D hit = Physics2D.Raycast(initialPos, rayDirection, maxArcherRange, raycastLayerMask);
                if(!hit || hit.transform != player.transform)
                    isAllHit = false;
            }

            return isAllHit;
        }
        return false;
    }

    public override void handleShotgunAttack(int dmg) {
        base.handleShotgunAttack(dmg);

        base.isKnockback = true;
        base.aiPath.canMove = false;
        an.SetBool("isWalking", false);

        isFireShot = false;
        isPreppingShot = false;
        shotPrepTimer = 0f;
        shotAnimationTimer = 0f;
    }

    public override void handleEnemyDeath() {
        base.handleEnemyDeath();
        Destroy(gameObject);
    }
}
