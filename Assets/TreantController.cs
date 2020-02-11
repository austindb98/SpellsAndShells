using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum TreantState {
    IdleLeft = 1,
    WalkingLeft = 2,
    IdleUp = 3,
    WalkingUp = 4,
    IdleRight = 5,
    WalkingRight = 6,
    IdleDown = 7,
    WalkingDown = 8
}

public class TreantController : MonoBehaviour
{
    private float speed = 2f;
    private float movementTimer = 0f;
    private Vector2 moveVelocity;
    private SpriteRenderer sr;
    private Animator an;
    private Rigidbody2D rb;

    void Start()
    {
        sr = gameObject.GetComponent<SpriteRenderer>(); 
        an = gameObject.GetComponent<Animator>();
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 moveInput;
        if(Input.GetAxis("Horizontal") != 0) {
            moveInput = new Vector2(Input.GetAxis("Horizontal"), 0);
            if(moveInput.x < 0)
                WalkLeft();
            else
                WalkRight();
        }
        else if(Input.GetAxis("Vertical") != 0) {
            moveInput = new Vector2(0, Input.GetAxis("Vertical"));
            if(moveInput.y < 0)
                WalkDown();
            else
                WalkUp();
        }
        else {
            moveInput = new Vector2(0, 0);
            an.SetBool("isTreantWalking", false);
        }
        moveVelocity = moveInput * speed;

    }

    void FixedUpdate() {
        rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);
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
