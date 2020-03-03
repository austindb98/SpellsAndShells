using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.Tilemaps;

public class EnemyController : MonoBehaviour
{

    public Animator an;
    public Collider2D playerCollider;
    public Rigidbody2D rb2d;

    public float attackStrength;

    public float knockbackStrength = 0.3f;

    public GameObject[] drops;
    public int dropChance;

    public bool isKnockback = false;
    private float knockbackTimer = 0f;
    public float knockbackTime = 0.8f;

    private bool isFrozen;
    private float frozenTimer;
    private float frozenTime;

    private bool isFlaming;
    private float flameTimer;
    private float flameTime;
    private float flameDamage;
    private float flameFrequency;

    //private float shotgunDamage = 3f;    // the damage taken by a single pellet of the shotgun

    public PlayerController playerController;

    public float knockbackCoefficient;

    public float maxSpeed;

    public GameObject player;
    public AIPath aiPath;

    public SpawnMaster spawnMaster;
    public SpawnManager spawnManager;

    public SpriteRenderer spriteRenderer;

    public EnemyHealth enemyHealth;

    virtual public void Start() {
        if(!player)
            player = GameObject.FindWithTag("Player");

        an = gameObject.GetComponent<Animator>();
        playerCollider = player.GetComponent<Collider2D>();
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        playerController = player.GetComponent<PlayerController>();
        if(aiPath != null) {    /* for enemies with A* pathfinding */
            maxSpeed = aiPath.maxSpeed;
            gameObject.GetComponent<AIDestinationSetter>().target = player.transform;
        }

        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        enemyHealth = gameObject.GetComponent<EnemyHealth>();
    }

    virtual public void Update() {
        if(isFlaming)
            handleFlaming();

        if(isFrozen)
            handleFrozen();

        if(isKnockback)
            handleKnockback();

    }

    virtual public void handleAttack(float damage, BaseAttack.Element element) {
        enemyHealth.takeDamage(damage, element);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Pot")
        {

            Tilemap tilemap = collision.gameObject.GetComponent<Tilemap>();
            Vector3 hitPosition = Vector3.zero;
            foreach (ContactPoint2D hit in collision.contacts)
            {
                hitPosition.x = hit.point.x - 0.01f * hit.normal.x;
                hitPosition.y = hit.point.y - 0.01f * hit.normal.y;
                //tilemap.SetTile(tilemap.WorldToCell(hitPosition), null);
                collision.gameObject.GetComponent<ObstacleController>().Break(hitPosition);
            }
        }
    }

    public void applyKnockback(float knockbackMagnitude) {
        Vector2 unitVec = transform.position - player.transform.position;
        unitVec.Normalize();
        rb2d.AddForce(unitVec * knockbackMagnitude);
        knockbackTimer = 0f;
        isKnockback = true;
    }

    virtual public void handleShotgunAttack(int shotgunDamage) {
        applyKnockback(knockbackStrength);
        enemyHealth.takeDamage(shotgunDamage, BaseAttack.Element.Normal);
    }

    virtual public void handleEnemyDeath() {
        float drop = Random.Range(0, drops.Length * dropChance);
        if(drop < drops.Length) {
            Instantiate(drops[(int)drop], transform.position, Quaternion.identity);
        }
        if(spawnMaster)
            spawnMaster.removeEnemyFromList(this);
        if(spawnManager)
            spawnManager.decrementEnemyCounter();
    }

    public virtual void applyFrostSlowingEffect(float magnitude, float time) {
        if(enemyHealth.weakness == BaseAttack.Element.Ice) {
            magnitude /= 1.5f;
            time *= 1.5f;
        }
        else if(enemyHealth.resistance == BaseAttack.Element.Ice) {
            magnitude *= 1.5f;
            time /= 1.5f;
        }
        aiPath.maxSpeed = maxSpeed * magnitude;
        isFrozen = true;
        frozenTime = time;
        frozenTimer = 0f;
        spriteRenderer.color = new Color(0.5f, 0.5f, 1, 1);
    }

    public virtual void applyFireDotEffect(float dotDuration, float dotFrequency, float dotDamage) {
        if(enemyHealth.weakness == BaseAttack.Element.Fire) {
            dotDamage *= 1.5f;
            dotDuration *= 1.5f;
        }
        else if(enemyHealth.resistance == BaseAttack.Element.Fire) {
            dotDamage /= 1.5f;
            dotDuration /= 1.5f;
        }
        isFlaming = true;
        flameTime = dotDuration;
        flameFrequency = dotFrequency;
        flameDamage = dotDamage;
        flameTimer = 0f;
        spriteRenderer.color = new Color(1, 0.5f, 0.5f, 1);

        if(isFrozen)
            cancelFrozen();
    }

    public virtual void applyWindKnockbackEffect(float knockbackMagnitude) {
        if(enemyHealth.weakness == BaseAttack.Element.Wind) {
            knockbackMagnitude *= 1.5f;
        }
        else if(enemyHealth.resistance == BaseAttack.Element.Wind) {
            knockbackMagnitude /= 1.5f;
        }
        applyKnockback(knockbackMagnitude);
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
        aiPath.canMove = false; // ?
        if(knockbackTimer > knockbackTime) {
            isKnockback = false;
            aiPath.canMove = true;
            knockbackTimer = 0f;
        }
    }
}
