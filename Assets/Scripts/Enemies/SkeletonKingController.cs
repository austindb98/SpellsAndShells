using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonKingController : EnemyController
{
    public GameObject skeletonPortalPrefab;

    public int numSpawnedSkeletons = 0;

    private bool isUsingAiPath = false;

    private bool isSwingRest = false;
    private float swingRestTime = 1f;
    private float swingRestTimer = 0f;

    private bool isDead = false;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        aiPath.canMove = false;
        disableColliders();

        knockbackStrength = 0.05f;
        knockbackTime = 0.3f;
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();

        UpdateDirection();

    }

    private void UpdateDirection() {
        float x;

        if(isSwingRest) {
            aiPath.canMove = false;
            swingRestTimer += Time.deltaTime;
            if(swingRestTimer > swingRestTime) {
                isSwingRest = false;
                swingRestTimer = 0f;
                an.SetBool("isWalk", true);
            }
        }

        if(!base.isKnockback && !isSwingRest && !isDead) {
            aiPath.canMove = true;
        }
        else {
            aiPath.canMove = false;
        }

        if(isUsingAiPath)
            x = aiPath.desiredVelocity.x;
        else
            x = rb2d.velocity.x;

        if(x < 0)
            transform.localScale = new Vector3(-10f, 10f, 1f);
        else
            transform.localScale = new Vector3(10f, 10f, 1f);
    }

    private void SummonSkeletons() {
        aiPath.canMove = false;
        rb2d.velocity = new Vector2(0, 0);
        an.SetBool("isDig", true);
        an.SetBool("isWalk", false);
        isUsingAiPath = false;
        disableColliders();
    }

    public void handleFinishDig() {
        spriteRenderer.enabled = false;
        GameObject portal = Instantiate(skeletonPortalPrefab, transform.position, new Quaternion(0, 0, 0, 0));
        (portal.GetComponent<SkeletonSpawner>()).skeletonKingController = this;
    }

    public void handleSkeletonDeath() {
        numSpawnedSkeletons--;
        if(numSpawnedSkeletons == 0) {
            an.SetBool("isDig", false);
            spriteRenderer.enabled = true;
        }
    }

    public void handleSpawnSkeleton() {
        numSpawnedSkeletons++;
    }

    public void handleFinishRise() {
        enableColliders();
        aiPath.canMove = true;
        an.SetBool("isWalk", true);
        isUsingAiPath = true;
    }

    public void handleAttack() {
        if (Vector3.Distance(transform.position, player.transform.position) < 4.0f)
        {
            playerController.takeDamage(attackStrength);
            playerController.onHitKnockback(1500.0f, transform.position);
        }
        isSwingRest = true;
        swingRestTimer = 0f;
        aiPath.canMove = false;
        an.SetBool("isAttack", false);
        an.SetBool("isWalk", false);
    }

    private void enableColliders() {
        Collider2D[] colliders = GetComponents<Collider2D>();
        colliders[0].enabled = true;
        colliders[1].enabled = true;
    }

    private void disableColliders() {
        Collider2D[] colliders = GetComponents<Collider2D>();
        colliders[0].enabled = false;
        colliders[1].enabled = false;
    }

    private void OnTriggerStay2D(Collider2D other) {
        if(other == playerCollider && !isKnockback) {
            if(!isSwingRest) {
                an.SetBool("isAttack", true);
                an.SetBool("isWalk", false);
            }
        }
    }
        
    private void OnTriggerEnter2D(Collider2D other) {
        if(other == playerCollider && !isKnockback) {
            if(!isSwingRest) {
                an.SetBool("isAttack", true);
                an.SetBool("isWalk", false);
            }
        }
    }
}
