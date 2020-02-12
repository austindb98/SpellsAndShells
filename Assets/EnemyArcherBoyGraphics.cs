using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Pathfinding;

public class EnemyArcherBoyGraphics : MonoBehaviour
{
    private float maxArcherRange = 15f;     // range from which archer can attack
    private float shotAnimationTime = 0.3f;// this is 100% guesswork. How to actually check this value?
    private float shotAnimationTimer;      // timer for shot animation sequence
    private bool isFireShot = false;        // indicates if in shot animation sequences
    private float shotPrepTime = 1f;        // time needed for shot cooldown
    private float shotPrepTimer;            // cooldown timer for shooting arrow
    private bool isPreppingShot = false;    // indicates if shot is being prepped

    private Animator an;
    public AIPath aiPath;
    public GameObject player;
    public GameObject arrow;

    void Start()
    {
        an = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isFireShot) {    // increment timer only if in animation sequences
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

        GameObject thisArrow = Instantiate(arrow, transform.position, q);
        thisArrow.GetComponent<Rigidbody2D>().velocity = 2.5f * (player.transform.position - transform.position);
    }

    // returns whether the player is in LoS of the ArcherBoy
    private bool CheckLineOfSight() {
        if(Vector3.Distance(player.transform.position, transform.position) < maxArcherRange) {
            Vector3 rayDirection = player.transform.position - transform.position;
            int raycastMask = ~((1 << 8) & (1 << 9) & (1 << 11));
            RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDirection, maxArcherRange, raycastMask);

            if (hit)
                return hit.transform == player.transform;
        }
        return false;
    }
}
