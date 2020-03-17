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
    public float dropChance;

    public bool isKnockback = false;
    private float knockbackTimer = 0f;
    public float knockbackTime = 0.8f;

    private bool isFrozen;
    private float frozenTimer;
    private float frozenTime;

    private bool isFlaming;
    private float flameTimer; //Current time on fire
    private float flameTimeLeft; //Length of the DOT
    private float flameDamage; //Damage per DOT tick
    private float flameFrequency; //Time between DOT ticks

    private float typeEffectivenessMultiplier = 1.5f;

    private Color blueTint = new Color(0.5f, 0.5f, 1, 1);
    private Color redTint = new Color(1, 0.5f, 0.5f, 1);
    private Color normalTint = new Color(1, 1, 1, 1);

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
    private bool attackHandled;

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
                // adjust for tile size
                hitPosition.x = hit.point.x - 0.01f * hit.normal.x;
                hitPosition.y = hit.point.y - 0.01f * hit.normal.y;
                //tilemap.SetTile(tilemap.WorldToCell(hitPosition), null);
                collision.gameObject.GetComponent<ObstacleController>().Break(hitPosition);
            }
        }
    }

    public virtual void applyKnockback(float knockbackMagnitude) {
        Vector2 unitVec = transform.position - player.transform.position;
        unitVec.Normalize();
        rb2d.AddForce(unitVec * knockbackMagnitude);
        knockbackTimer = 0f;
        isKnockback = true;
    }

    virtual public void handleShotgunAttack(int shotgunDamage) {
        applyKnockback(knockbackStrength);
        if(enemyHealth)
            enemyHealth.takeDamage(shotgunDamage, BaseAttack.Element.Normal);
    }

    virtual public void handleEnemyDeath() {
        dropChance = Mathf.Clamp(dropChance,0,1);
        float drop = Random.Range(0, drops.Length / dropChance);
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
            magnitude /= typeEffectivenessMultiplier;
            time *= typeEffectivenessMultiplier;
        }
        else if(enemyHealth.resistance == BaseAttack.Element.Ice) {
            magnitude *= typeEffectivenessMultiplier;
            time /= typeEffectivenessMultiplier;
        }
        aiPath.maxSpeed = maxSpeed * magnitude;
        isFrozen = true;
        frozenTime = time;
        frozenTimer = 0f;
        spriteRenderer.color = blueTint;
    }

    public virtual void applyFireDotEffect(float dotDuration, float dotFrequency, float dotDamage) {
        if(enemyHealth.weakness == BaseAttack.Element.Fire) {
            dotDamage *= typeEffectivenessMultiplier; // TODO: no magic numbers
            dotDuration *= typeEffectivenessMultiplier;
        }
        else if(enemyHealth.resistance == BaseAttack.Element.Fire) {
            dotDamage /= typeEffectivenessMultiplier;
            dotDuration /= typeEffectivenessMultiplier;
        }
        isFlaming = true;
        flameTimeLeft = dotDuration;
        flameFrequency = dotFrequency;
        flameDamage = dotDamage;
        flameTimer = 0f;
        spriteRenderer.color = redTint;

        if(isFrozen)
            cancelFrozen();
    }

    public virtual void applyWindKnockbackEffect(float knockbackMagnitude) {
        if(enemyHealth.weakness == BaseAttack.Element.Wind) {
            knockbackMagnitude *= typeEffectivenessMultiplier;
        }
        else if(enemyHealth.resistance == BaseAttack.Element.Wind) {
            knockbackMagnitude /= typeEffectivenessMultiplier;
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
        if(flameTimer >= flameFrequency) {
            enemyHealth.takeDamage(flameDamage, BaseAttack.Element.Fire);
            flameTimer = 0;
            flameTimeLeft -= flameFrequency;
        }
        if(flameTimeLeft <= 0)
            cancelFlaming();
    }

    private void cancelFlaming() {
        isFlaming = false;
        spriteRenderer.color = normalTint;
    }

    protected void cancelFrozen() {
        isFrozen = false;
        aiPath.maxSpeed = maxSpeed;
        spriteRenderer.color = normalTint;
    }

    private void handleKnockback() {
        if(!aiPath)
            return;
        knockbackTimer += Time.deltaTime;
        aiPath.canMove = false;
        if(knockbackTimer > knockbackTime) {
            isKnockback = false;
            aiPath.canMove = true;
            knockbackTimer = 0f;
        }
    }
}
