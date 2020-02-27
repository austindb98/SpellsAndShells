using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyController : MonoBehaviour
{

    public Animator an;
    public Collider2D playerCollider;
    public Rigidbody2D rb2d;

    public float attackStrength;

    public GameObject[] drops;
    public int dropChance;

    public bool isKnockback = false;
    private float knockbackTimer = 0f;
    private float knockbackTime = 0.8f;
    
    private bool isFrozen;
    private float frozenTimer;
    private float frozenTime;

    private bool isFlaming;
    private float flameTimer;
    private float flameTime;
    private float flameDamage;
    private float flameFrequency;

    public PlayerController playerController;

    public float knockbackCoefficient;

    public float maxSpeed;

    public GameObject player;
    public AIPath aiPath;

    public SpawnMaster spawnMaster;

    public SpriteRenderer spriteRenderer;

    public EnemyHealth enemyHealth;

    virtual public void Start() {
        if(!player)
            player = GameObject.FindWithTag("Player");

        an = gameObject.GetComponent<Animator>();
        playerCollider = player.GetComponent<Collider2D>();
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        playerController = player.GetComponent<PlayerController>();
        maxSpeed = aiPath.maxSpeed;

        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        enemyHealth = gameObject.GetComponent<EnemyHealth>();
        gameObject.GetComponent<AIDestinationSetter>().target = player.transform;
    }

    virtual public void Update() {
        if(isFlaming)
            handleFlaming();

        if(isFrozen)
            handleFrozen();

        if(isKnockback)
            handleKnockback();

    }

    virtual public void handleShotgunHit(float knockbackMagnitude) {
        Vector2 unitVec = transform.position - player.transform.position;
        unitVec.Normalize();
        rb2d.AddForce(unitVec * knockbackMagnitude);
    }

    virtual public void handleEnemyDeath() {
        float drop = Random.Range(0, drops.Length * dropChance);
        if(drop < drops.Length) {
            Instantiate(drops[(int)drop], transform.position, Quaternion.identity);
        }
        if(spawnMaster)
            spawnMaster.removeEnemyFromList(this);
    }

    public void applyFrostSlowingEffect(float magnitude, float time) {
        aiPath.maxSpeed *= magnitude;
        isFrozen = true;
        frozenTime = time;
        frozenTimer = 0f;
        spriteRenderer.color = new Color(0.5f, 0.5f, 1, 1);
    }

    public void applyFireDotEffect(float dotDuration, float dotFrequency, float dotDamage) {
        isFlaming = true;
        flameTime = dotDuration;
        flameFrequency = dotFrequency;
        flameDamage = dotDamage;
        flameTimer = 0f;
        spriteRenderer.color = new Color(1, 0.5f, 0.5f, 1);

        if(isFrozen)
            cancelFrozen();
    }

    private void handleFrozen() {
        frozenTimer += Time.deltaTime;
        if(frozenTimer > frozenTime)
            cancelFrozen();
    }

    private void handleFlaming() {
        flameTimer += Time.deltaTime;
        if(flameTimer > flameFrequency) {
            enemyHealth.takeDamage(flameDamage, BaseAttack.Element.Fire);
            flameFrequency += flameFrequency;
        }
        if(flameTimer > flameTime)
            cancelFlaming();
    }

    private void cancelFlaming() {
        isFlaming = false;
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }
    
    private void cancelFrozen() {
        isFrozen = false;
        aiPath.maxSpeed = maxSpeed;
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }

    private void handleKnockback() {
        knockbackTimer += Time.deltaTime;
        if(knockbackTimer > knockbackTime) {
            isKnockback = false;
            aiPath.canMove = true;
            knockbackTimer = 0f;
        }
    }
}
