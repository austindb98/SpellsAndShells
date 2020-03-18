using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonKingController : EnemyController
{
    public GameObject skeletonPortalPrefab;

    private int numSummons = 0;
    private int numDeaths = 0;

    public RectTransform healthBar;

    public int numSpawnedSkeletons = 0;

    private bool isUsingAiPath = false;

    private bool isSwingRest = false;
    private float swingRestTime = 1f;
    private float swingRestTimer = 0f;

    private float maxLungeRange = 10f;
    private bool isLunge = false;
    private float maxLungeTime = 1f;
    private float lungeTimer = 0f;
    private bool isJumpAttack = false;

    private bool isDead = false;
    private bool isUnderground = true;

    private bool isReviving;
    private float reviveTimer = 0f;
    private float reviveTime = 5f;

    private int raycastLayerMask;

    public float maxHealth = 1200f; //1200
    public float bossHealth = 1200f;

    private System.Random rnd;
    public FinalSceneController sceneController;
    private bool isCompletelyDead = false;  

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        aiPath.canMove = false;
        disableColliders();

        knockbackStrength = 0.05f;
        knockbackTime = 0.3f;
        attackStrength = 25f;

        rnd = new System.Random();

        raycastLayerMask =  ((1 << LayerMask.NameToLayer("Obstacles")) |
                             (1 << LayerMask.NameToLayer("Walls")) |
                             (1 << LayerMask.NameToLayer("Player")));

        sceneController = GameObject.FindWithTag("BossRoom").GetComponent<FinalSceneController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isCompletelyDead)
            return;
        base.Update();

        UpdateDirection();

    }

    private void UpdateDirection() {
        float x;

        if(isUnderground)
            return;
        else if(isDead) {
            reviveTimer += Time.deltaTime;
            if(reviveTimer > reviveTime) {
                StartRevival();
            }
        }
        else if(isSwingRest) {
            aiPath.canMove = false;
            swingRestTimer += Time.deltaTime;
            if(swingRestTimer > swingRestTime) {
                isSwingRest = false;
                swingRestTimer = 0f;
                DisableAllAnimationVars();
                an.SetBool("isWalk", true);
                aiPath.canMove = true;
            }
        }
        else if(!isLunge && numSummons < numDeaths && rnd.NextDouble() < 0.2f * Time.deltaTime) {
            SummonSkeletons();
        }
        else if(!isLunge && rnd.NextDouble() < 0.25f * Time.deltaTime) {
            if(CheckLineOfSight()) {    /* start lunge sequence */
                cancelFrozen();
                aiPath.canMove = false;
                isUsingAiPath = false;
                isLunge = true;
                lungeTimer = 0f;
                
                Vector2 newVelocity = new Vector2(player.transform.position.x - transform.position.x,
                                                  player.transform.position.y - transform.position.y);
                newVelocity.Normalize();
                rb2d.velocity = 25f * newVelocity;
                rb2d.drag = 1;
            }
        }
        else if(isLunge) {
            if((player.transform.position - transform.position).magnitude < 5f) {
                DisableAllAnimationVars();
                an.SetBool("isJumpAttack", true);
                rb2d.drag = 4;

                isLunge = false;
                isJumpAttack = true;
            }
            else {
                lungeTimer += Time.deltaTime;
                if(lungeTimer > maxLungeTime) { // stop lunge attack, not close enough. This is a failsafe
                    isLunge = false;
                    isUsingAiPath = true;
                    rb2d.drag = 4;
                }
            }
            
        }


        // conditional ensures there are no weird edge cases that allow the king to move when he shouldn't
        if(!base.isKnockback && !isSwingRest && !isDead && !isUnderground && !isLunge && !isJumpAttack)
            aiPath.canMove = true;
        else 
            aiPath.canMove = false;

        if(isUsingAiPath)
            x = aiPath.desiredVelocity.x;
        else
            x = rb2d.velocity.x;

        if(x < 0)
            transform.localScale = new Vector3(-10f, 10f, 1f);
        else
            transform.localScale = new Vector3(10f, 10f, 1f);
    }

    override public void handleAttack(float damage, BaseAttack.Element element) {
        base.handleAttack(damage, element);
        updateBossHealth(damage, element);
        isJumpAttack = false;
    }

    override public void handleShotgunAttack(int shotgunDamage) {
        if(isCompletelyDead) {
            sceneController.handleShootSkeletonKing();
            Destroy(gameObject);
        }
        else {
            base.handleShotgunAttack(shotgunDamage);
            updateBossHealth(shotgunDamage, BaseAttack.Element.Normal);
            isJumpAttack = false;
        }
    }

    private void updateBossHealth(float damage, BaseAttack.Element element) {
        if(isDead && !isReviving) {
            SetHealth(bossHealth - base.enemyHealth.calculateDamageTaken(damage, element));
            if(bossHealth <= 0)
                handleFinishBossFight();
        }
    }

    private void handleFinishBossFight() {
        if(isCompletelyDead)
            return;
        sceneController.handleSkeletonKingDown(transform.position);
        isCompletelyDead = true;
        print("done!");
    }

    private void SummonSkeletons() {
        DisableAllAnimationVars();
        aiPath.canMove = false;
        rb2d.velocity = new Vector2(0, 0);
        an.SetBool("isDig", true);
        isUsingAiPath = false;
        isUnderground = true;
        disableColliders();
        numSummons++;
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
        isJumpAttack = false;
        isLunge = false;
        aiPath.canMove = true;
        an.SetBool("isWalk", true);
        isUsingAiPath = true;
        isUnderground = false;
    }

    public void handleNormalAttack() {
        if (Vector3.Distance(transform.position, player.transform.position) < 4.0f)
        {
            playerController.takeDamage(attackStrength);
            playerController.onHitKnockback(1500.0f, transform.position);
        }
        isSwingRest = true;
        swingRestTimer = 0f;
        aiPath.canMove = false;
        DisableAllAnimationVars();
    }

    public void handleJumpAttack() {
        if (Vector3.Distance(transform.position, player.transform.position) < 4.0f)
        {
            playerController.takeDamage(attackStrength);
            playerController.onHitKnockback(1500.0f, transform.position);
        }
        isJumpAttack = false;
        isSwingRest = true;
        swingRestTimer = 0f;
        aiPath.canMove = false;
        isUsingAiPath = true;
        DisableAllAnimationVars();
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
        if(other == playerCollider && !isKnockback && !isDead && !isLunge && !isJumpAttack) {
            if(!isSwingRest) {
                DisableAllAnimationVars();
                an.SetBool("isAttack", true);
            }
        }
    }
        
    private void OnTriggerEnter2D(Collider2D other) {
        if(other == playerCollider && !isKnockback && !isDead && !isLunge && !isJumpAttack) {
            if(!isSwingRest) {
                DisableAllAnimationVars();
                an.SetBool("isAttack", true);
            }
        }
    }

        // returns whether the player is in LoS of the ArcherBoy
    private bool CheckLineOfSight() {
        bool isAllHit = true;

        if(Vector3.Distance(player.transform.position, transform.position) < maxLungeRange) {
            Vector3[] rayStartingPoints = {
                transform.position + new Vector3(1.5f, 0, 0),
                transform.position + new Vector3(-1.5f, 0, 0),
                transform.position + new Vector3(0, 1.5f, 0),
                transform.position + new Vector3(0, -1.5f, 0)
            };
            foreach(Vector3 initialPos in rayStartingPoints) {
                Vector3 rayDirection = player.transform.position - initialPos;
                RaycastHit2D hit = Physics2D.Raycast(initialPos, rayDirection, maxLungeRange, raycastLayerMask);
                if(!hit || hit.transform != player.transform)
                    isAllHit = false;
            }

            return isAllHit;
        }
        return false;
    }

    public void FinishRevival() {
        aiPath.canMove = true;
        an.SetBool("isWalk", true);
        isReviving = false;
        rb2d.constraints &= ~RigidbodyConstraints2D.FreezePosition;
        isDead = false;
    }

    private void StartRevival() {
        isReviving = true;  
        an.SetBool("isDeath", false);
        enemyHealth.setCurrentHealth(enemyHealth.maxHealth);
        aiPath.canMove = false;
    }

    public override void handleEnemyDeath() {
        if (isDead)
            return;
        aiPath.canMove = false;
        DisableAllAnimationVars();
        an.SetBool("isDeath", true);

        reviveTimer = 0f;
        isDead = true;

        rb2d.constraints |= RigidbodyConstraints2D.FreezePosition;
        enemyHealth.setCurrentHealth(10000f);   // arbitrary high value
        numDeaths++;
    }

    private void DisableAllAnimationVars() {
        an.SetBool("isDeath", false);
        an.SetBool("isWalk", false);
        an.SetBool("isIdle", false);
        an.SetBool("isAttack", false);
        an.SetBool("isJumpAttack", false);
        an.SetBool("isDig", false);
    }

    public void SetHealth(float newHealth)
    {
        if (newHealth < 0)
            newHealth = 0;
        bossHealth = newHealth;
        healthBar.localScale = new Vector3(bossHealth / maxHealth, 1, 1);
    }
}
