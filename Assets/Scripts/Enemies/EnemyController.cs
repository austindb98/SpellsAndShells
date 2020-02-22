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

    public bool isKnockback = false;
    private float knockbackTimer = 0f;
    private float knockbackTime = 0.8f;

    public PlayerController playerController;

    public float knockbackCoefficient;

    public GameObject player;
    public AIPath aiPath;

    public SpawnMaster spawnMaster;

    virtual public void Start() {
        if(!player)
                player = GameObject.FindWithTag("Player");

        an = gameObject.GetComponent<Animator>();
        playerCollider = player.GetComponent<Collider2D>();
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        playerController = player.GetComponent<PlayerController>();

        gameObject.GetComponent<AIDestinationSetter>().target = player.transform;
    }

    virtual public void Update() {
        if(isKnockback) {
            knockbackTimer += Time.deltaTime;
            if(knockbackTimer > knockbackTime) {
                isKnockback = false;
                aiPath.canMove = true;
                knockbackTimer = 0f;
            }
        }

    }
    
    virtual public void handleShotgunHit(float knockbackMagnitude) {
        Vector2 unitVec = transform.position - player.transform.position;
        unitVec.Normalize();
        rb2d.AddForce(unitVec * knockbackMagnitude);
    }

    virtual public void handleEnemyDeath() {
        if(spawnMaster)
            spawnMaster.removeEnemyFromList(this);
    }
}
