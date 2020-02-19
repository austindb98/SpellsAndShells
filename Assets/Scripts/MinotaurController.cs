using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Pathfinding;

public class MinotaurController : EnemyController
{
    private Collider2D playerCollider;
    private Rigidbody2D rb2d;
    private bool isKnockback = false;
    private float knockbackTimer = 0f;
    private float knockbackTime = 0.8f;
    private PlayerController playerController;

    private bool isSwingRest = false;
    private float swingRestTime = 1f;
    private float swingRestTimer = 0f;

    public GameObject player;
    private Animator an;
    public AIPath aiPath;
    // Start is called before the first frame update
    public override void Start()
    {
        player = GameObject.FindWithTag("Player");
        gameObject.GetComponent<AIDestinationSetter>().target = player.transform;
        an = gameObject.GetComponent<Animator>();

        playerCollider = player.GetComponent<Collider2D>();
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        playerController = player.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    public override void Update()
    {        
        float x = aiPath.desiredVelocity.x;
        float y = aiPath.desiredVelocity.y;

        if(isKnockback || isSwingRest) {
            if(isSwingRest) {
                swingRestTimer += Time.deltaTime;
                if(swingRestTimer > swingRestTime) {
                    isSwingRest = false;
                    swingRestTimer = 0f;
                }
            }
            if(isKnockback) {
                knockbackTimer += Time.deltaTime;
                if(knockbackTimer > knockbackTime) {
                    isKnockback = false;
                    knockbackTimer = 0f;
                }
            }
            if(!isKnockback && !isSwingRest) {
                aiPath.canMove = true;
            }
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
        {
            an.SetBool("isWalking", true);
        }
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other == playerCollider && !isKnockback) {
            if(!isSwingRest)
                an.SetBool("isAttack1", true);
        }
    }

    public override void handleShotgunHit(float knockbackMagnitude) {
        Vector2 unitVec = transform.position - player.transform.position;
        unitVec.Normalize();
        rb2d.AddForce(unitVec * knockbackMagnitude);
        isKnockback = true;
        aiPath.canMove = false;
        an.SetBool("isWalking", false);
        an.SetBool("isAttack1", false);
    }

    public override void handleEnemyDeath() {
        Destroy(gameObject);
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
        playerController.takeDamage(10f);
        isSwingRest = true;
        swingRestTimer = 0f;
        aiPath.canMove = false;
        an.SetBool("isAttack1", false);
    }
}
