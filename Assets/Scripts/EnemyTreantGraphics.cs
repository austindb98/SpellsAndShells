using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Pathfinding;

public class EnemyTreantGraphics : MonoBehaviour
{
    private Animator an;
    private Collider2D playerCollider;
    private Rigidbody2D rb2d;
    private bool isKnockback = false;
    private float knockbackTimer = 0f;
    private float knockbackTime = 0.8f;
    private PlayerController playerController;

    public GameObject player;
    public AIPath aiPath;

    void Start()
    {
        an = gameObject.GetComponent<Animator>();
        playerCollider = player.GetComponent<Collider2D>();
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        playerController = player.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        float x = aiPath.desiredVelocity.x;
        float y = aiPath.desiredVelocity.y;

        if(isKnockback) {
            knockbackTimer += Time.deltaTime;
            if(knockbackTimer > knockbackTime) {
                isKnockback = false;
                aiPath.canMove = true;
                knockbackTimer = 0f;
            }
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
        if(other == playerCollider) {
            Vector2 unitVec = transform.position - player.transform.position;
            unitVec.Normalize();
            rb2d.AddForce(unitVec * 1f);
            isKnockback = true;
            aiPath.canMove = false;
            an.SetBool("isTreantWalking", false);

            playerController.takeDamage(20f);
        }
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
