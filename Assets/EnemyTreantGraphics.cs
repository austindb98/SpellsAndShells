using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Pathfinding;

public class EnemyTreantGraphics : MonoBehaviour
{
    private float speed = 2f;
    private float movementTimer = 0f;
    private Vector2 moveVelocity;
    private SpriteRenderer sr;
    private Animator an;
    private Rigidbody2D rb;

    public AIPath aiPath;

    void Start()
    {
        sr = gameObject.GetComponent<SpriteRenderer>(); 
        an = gameObject.GetComponent<Animator>();
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float x = aiPath.desiredVelocity.x;
        float y = aiPath.desiredVelocity.y;

        if(x == 0 && y == 0) {
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
