using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatController : EnemyController
{
    private enum BatIdleStates {
        PauseLeft, MoveLeft, PauseRight, MoveRight
    }
    private BatIdleStates batIdleState;
    private float batMaxRange = 12f;
    private bool isSeePlayer;

    private float pauseTimer;
    private float pauseTime = 2f;
    private float moveTimer;
    private float moveTime = 1f;

    private Vector3 moveLeftVector;
    private Vector3 moveRightVector;
    private Vector3 stationaryVector;

    private int raycastLayerMask;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();

        attackStrength = 15f;
        aiPath.canMove = false;

        moveLeftVector = new Vector3(-4f, 0, 0);
        moveRightVector = new Vector3(4f, 0, 0);
        stationaryVector = new Vector3(0, 0, 0);

        batIdleState = BatIdleStates.PauseLeft;

        raycastLayerMask =  ((1 << LayerMask.NameToLayer("Obstacles")) |
                             (1 << LayerMask.NameToLayer("Walls")) |
                             (1 << LayerMask.NameToLayer("Player")));
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();

        if(CheckLineOfSight()) {
            if(!isSeePlayer) {  /* bat sees player and didn't on last update */
                isSeePlayer = true;
                an.SetBool("isAngry", true);
                aiPath.canMove = true;
            }
        }
        else {
            if(isSeePlayer) {  /* bat doesn't see player and did on last update */
                isSeePlayer = false;
                an.SetBool("isAngry", false);
                aiPath.canMove = false;
            }
            BatIdle();
        }
    }

    private void BatIdle() {
        if(batIdleState == BatIdleStates.PauseLeft || batIdleState == BatIdleStates.PauseRight) {
            pauseTimer += Time.deltaTime;
            print("pauseTimer: " + pauseTimer);
            if(pauseTimer > pauseTime) {
                if(batIdleState == BatIdleStates.PauseLeft)
                    transitionBatMoveRight();
                else
                    transitionBatMoveLeft();
            }
        }
        else {
            if(batIdleState == BatIdleStates.MoveRight)
                rb2d.velocity = moveRightVector;
            else if(batIdleState == BatIdleStates.MoveLeft)
                rb2d.velocity = moveLeftVector;
            moveTimer += Time.deltaTime;
            print("moveTimer: " + moveTimer);
            if(moveTimer > moveTime) {
                if(batIdleState == BatIdleStates.MoveLeft)
                    transitionBatIdleLeft();
                else
                    transitionBatIdleRight();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other == playerCollider && !isKnockback)
        {
            applyKnockback(1f);
            playerController.takeDamage(attackStrength);
        }
    }

    private void transitionBatMoveRight() {
        moveTimer = 0f;
        batIdleState = BatIdleStates.MoveRight;
        rb2d.velocity = moveRightVector;
    }

    private void transitionBatMoveLeft() {
        moveTimer = 0f;
        batIdleState = BatIdleStates.MoveLeft;
        rb2d.velocity = moveLeftVector;
    }

    private void transitionBatIdleRight() {
        pauseTimer = 0f;
        batIdleState = BatIdleStates.PauseRight;
        rb2d.velocity = stationaryVector;
    }

    private void transitionBatIdleLeft() {
        pauseTimer = 0f;
        batIdleState = BatIdleStates.PauseLeft;
        rb2d.velocity = stationaryVector;
    }

    // returns whether the player is in LoS of the ArcherBoy
    private bool CheckLineOfSight() {
        bool isHit = false;

        if(Vector3.Distance(player.transform.position, transform.position) < batMaxRange) {
            Vector3[] rayStartingPoints = {
                transform.position + new Vector3(0.5f, 0, 0),
                transform.position + new Vector3(-0.5f, 0, 0),
                transform.position + new Vector3(0, 0.5f, 0),
                transform.position + new Vector3(0, -0.5f, 0)
            };
            foreach(Vector3 initialPos in rayStartingPoints) {
                Vector3 rayDirection = player.transform.position - initialPos;
                RaycastHit2D hit = Physics2D.Raycast(initialPos, rayDirection, batMaxRange, raycastLayerMask);
                if(hit && hit.transform == player.transform)
                    isHit = true;
            }

            return isHit;
        }
        return false;
    }
}
