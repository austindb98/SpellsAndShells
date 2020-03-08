using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Pathfinding;

public class SkeletalArcherController : EnemyController
{
    private float maxArcherRange = 20f;     // range from which archer can attack
    private int raycastLayerMask;
    private float arrowSpeed = 20f;

    private float reviveTimer = 0f;
    private float reviveTime = 5f;
    private bool isDead = false;
    private bool isReviving = false;

    private bool isCooldown = false;
    private float cooldownTime = 2f;
    private float cooldownTimer = 0f;

    public GameObject arrow;

    public override void Start()
    {
        base.Start();
        raycastLayerMask =  ((1 << LayerMask.NameToLayer("Obstacles")) |
                             (1 << LayerMask.NameToLayer("Walls")) |
                             (1 << LayerMask.NameToLayer("Player")));
        float x = player.transform.position.x - transform.position.x;
        an.SetBool("isFacingRight", x > 0);

        knockbackStrength = 0.1f;
        knockbackTime = 0.4f;
    }

    // Update is called once per frame
    public override void Update()
    {
        float x = player.transform.position.x - transform.position.x;
        base.Update();

        if(isCooldown) {
            cooldownTimer += Time.deltaTime;
            if(cooldownTimer > cooldownTime) {
                cooldownTimer = 0f;
                isCooldown = false;
            }
        }

        if(isDead) {
            reviveTimer += Time.deltaTime;
            if(reviveTimer > reviveTime) {
                StartRevival();
            }
        }
        else if(isKnockback) {
            return;
        }
        else if(!isCooldown && CheckLineOfSight()) {    // target is in LoS. prep shot
            aiPath.canMove = false;
            an.SetBool("isWalking", false);
            an.SetBool("isAttack1", true);
        }
        else if(aiPath.desiredVelocity.x == 0 && aiPath.desiredVelocity.y == 0) { // idle
            an.SetBool("isWalking", false);
        }
        else if(x > 0) {
            an.SetBool("isFacingRight", true);
            an.SetBool("isWalking", true);
            aiPath.canMove = true;
        }
        else if(x < 0) {
            an.SetBool("isFacingRight", false);
            an.SetBool("isWalking", true);
            aiPath.canMove = true;
        }
        else {
            an.SetBool("isWalking", true);
            aiPath.canMove = true;
        }
    }

    public void handleShoot() {
        Vector3 dir = getArrowDirection();
        float angle = Mathf.Atan2( dir.y, dir.x )  * Mathf.Rad2Deg - 90;
        Quaternion q = Quaternion.Euler( 0f, 0f, angle );

        GameObject thisArrow = Instantiate(arrow, transform.position, q);
        thisArrow.GetComponent<Rigidbody2D>().velocity = 30f * dir;
        thisArrow.GetComponent<ArrowController>().player = player;
    }

    private Vector3 getArrowDirection() {
        float deltaX = player.transform.position.x - transform.position.x;
        float deltaY = player.transform.position.y - transform.position.y;
        float playerVX = player.GetComponent<Rigidbody2D>().velocity.x;
        float playerVY = player.GetComponent<Rigidbody2D>().velocity.y;

        float a = playerVX * playerVX + playerVY * playerVY - arrowSpeed * arrowSpeed;
        float b = deltaX * playerVX + deltaY * playerVY;
        float c = deltaX * deltaX + deltaY * deltaY;

        double time = (-1 * b - Math.Sqrt(b * b - 4 * a * c)) / (2 * a);
        // get qUaDraTiC

        Vector3 predictionVec = new Vector3((float) (deltaX / time + playerVX), (float) (deltaY / time + playerVY), 0);
        predictionVec.Normalize();
        Vector3 playerDirection = player.transform.position - transform.position;
        playerDirection.Normalize();
        Vector3 arrowDirection = predictionVec + 2f * playerDirection;
        arrowDirection.Normalize();
        return arrowDirection;

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

    public override void handleShotgunAttack(int dmg) {
        base.handleShotgunAttack(dmg);

        base.isKnockback = true;
        base.aiPath.canMove = false;
        an.SetBool("isWalking", false);
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

    public void handleFinishAttack() {
        an.SetBool("isAttack1", false);
        aiPath.canMove = true;
        isCooldown = true;
    }
}
