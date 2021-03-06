﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Pathfinding;

public class WitchController : EnemyController
{
    private float maxHexRange = 25f;     // range from which archer can attack
    private int raycastLayerMask;
    private float spellSpeed = 15f;

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
        float x = player.transform.position.x - transform.position.x;
        an.SetBool("isFacingRight", x > 0);
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
            //an.SetBool("isFacingRight", x > 0);
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

    /*private void triSpellCast() {
        Vector3 atPlayerUnitVec = player.transform.position - transform.position;
        Vector3 predictiveUnitVec = getUnitVec();//player.transform.position - transform.position;
        predictiveUnitVec.Normalize();
        atPlayerUnitVec.Normalize();

        Vector3 betweenPlayerUnitVec = predictiveUnitVec + atPlayerUnitVec;
        betweenPlayerUnitVec.Normalize();

        castSpell(atPlayerUnitVec);
        castSpell(predictiveUnitVec);
        castSpell(betweenPlayerUnitVec);
    }*/

    private void castSpell(Vector3 unitVec) {
        float angle = Mathf.Atan2( unitVec.y, unitVec.x )  * Mathf.Rad2Deg + 90;
        Quaternion q = Quaternion.Euler( 0f, 0f, angle );
        unitVec.Normalize();

        GameObject thisArrow = Instantiate(spell, transform.position, q);
        thisArrow.GetComponent<Rigidbody2D>().velocity = spellSpeed * unitVec;
        thisArrow.GetComponent<WitchSpellController>().player = player;
        thisArrow.GetComponent<WitchSpellController>().speed = spellSpeed;
        an.SetBool("isHexing", false);
        base.aiPath.canMove = true;
    }

    private Vector3 getUnitVec() {
        float deltaX = player.transform.position.x - transform.position.x;
        float deltaY = player.transform.position.y - transform.position.y;
        float playerVX = player.GetComponent<Rigidbody2D>().velocity.x;
        float playerVY = player.GetComponent<Rigidbody2D>().velocity.y;

        float a = playerVX * playerVX + playerVY * playerVY - spellSpeed * spellSpeed;
        float b = deltaX * playerVX + deltaY * playerVY;
        float c = deltaX * deltaX + deltaY * deltaY;

        double time = (-1 * b - Math.Sqrt(b * b - 4 * a * c)) / (2 * a);
        // get qUaDraTiC

        Vector3 vec = new Vector3((float) (deltaX / time + playerVX), (float) (deltaY / time + playerVY), 0);
        vec.Normalize();
        return vec;

    }

    // returns whether the player is in LoS of the ArcherBoy
    private bool CheckLineOfSight() {
        bool isAllHit = true;

        if(Vector3.Distance(player.transform.position, transform.position) < maxHexRange) {
            Vector3[] rayStartingPoints = {
                transform.position + new Vector3(0.5f, 0, 0),
                transform.position + new Vector3(-0.5f, 0, 0),
                transform.position + new Vector3(0, 0.5f, 0),
                transform.position + new Vector3(0, -0.5f, 0)
            };
            foreach(Vector3 initialPos in rayStartingPoints) {
                Vector3 rayDirection = player.transform.position - initialPos;
                RaycastHit2D hit = Physics2D.Raycast(initialPos, rayDirection, maxHexRange, raycastLayerMask);
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
    }

    public void handleHex() {
        // hex was just cast
        Vector3 playerPositionVector = player.transform.position - transform.position;
        playerPositionVector.Normalize();
        castSpell(getUnitVec() + playerPositionVector);
        float x = player.transform.position.x - transform.position.x;
        an.SetBool("isFacingRight", x > 0);
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
