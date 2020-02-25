using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Pathfinding;

public class CobraController : EnemyController
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
        player = GameObject.FindWithTag("Player");
        gameObject.GetComponent<AIDestinationSetter>().target = player.transform;
        an = gameObject.GetComponent<Animator>();
        attackStrength = 15;
        playerCollider = player.GetComponent<Collider2D>();
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        playerController = player.GetComponent<PlayerController>();
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
        
        if(!base.isKnockback && !isSwingRest && !isDead)
            aiPath.canMove = true;
        else
            aiPath.canMove = false;

        if(base.isKnockback || isSwingRest)
            return;
        else if (aiPath.desiredVelocity.x == 0 && aiPath.desiredVelocity.y == 0)
            an.SetBool("isWalking", false);
        else if (x > 0)
            WalkRight();
        else if (x < 0)
            WalkLeft();
        else
            an.SetBool("isWalking", true);
        

        if(isDead) {
            deathTimer += Time.deltaTime;
            if(deathTimer > deathTime) {
                base.handleEnemyDeath();
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other == playerCollider && !isKnockback)
        {
            if (!isSwingRest)
                an.SetBool("isAttack1", true);
        }
    }

    public override void handleShotgunHit(float knockbackMagnitude)
    {
        Vector2 unitVec = transform.position - player.transform.position;
        unitVec.Normalize();
        if (!isDead)
            rb2d.AddForce(unitVec * knockbackMagnitude * 0.7f);
        isKnockback = true;
        aiPath.canMove = false;
        an.SetBool("isWalking", false);
        an.SetBool("isAttack1", false);
    }

    public override void handleEnemyDeath()
    {
        if (isDead)
        {
            return; // so animation doesn't keep on playing
        }
        aiPath.canMove = false;
        an.SetBool("isDead", true);
        isDead = true;
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

    public void handleAttack()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < 2.0f)
        {
            playerController.takeDamage(attackStrength);
            playerController.onHitKnockback(800.0f, transform.position);
        }
        isSwingRest = true;
        swingRestTimer = 0f;
        aiPath.canMove = false;
        an.SetBool("isAttack1", false);
    }
}
