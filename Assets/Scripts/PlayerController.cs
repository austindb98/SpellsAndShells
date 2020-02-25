using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using System;

public class PlayerController : BasePlayer {

    public class Shot {
        public int pellets, damage;
        public float variance, range;
        public Shot(int pellets, float variance, float range, int damage) {
            this.pellets = pellets;
            this.variance = variance;
            this.range = range;
            this.damage = damage;
        }
    }

    public Shot[] Shells = {
        new Shot(10,0.5f,20f,5),
        new Shot(1,0f,30f,75),
        new Shot(25,1f,15f,3),
    };

    public float speed;
    private float baseSpeed;
    public float angle;
    public Sprite frontLeft, diagUpLeft, back, diagUpRight, frontRight, diagDownRight, frontDown, diagDownLeft;
    public Sprite cursor;
    public ParticleSystem shotgunBlast, shotgunPellets;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb2d;
    private bool isHit = false;
    private float hitTimer = 0f;
    private float hitTime = 0.5f;
    private float knockbackTimer = 0;
    private float knockbackTime = 0.8f;
    private bool isKnockback = false;
    
    public LayerMask wallLayer, obstacleLayer, fogLayer;
    private LayerMask interactsWithBullets;
    private Vector2 mousePos;
    private float heartHealth = 30f;
    private float potionMana = 30f;

    public SpawnMaster initialSpawnMaster; // this will not be necessary in the final design

    protected override void Start() {
        base.Start();
        rb2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        interactsWithBullets =  wallLayer |
                                obstacleLayer |
                                (1 << LayerMask.NameToLayer("Entities")) |
                                (1 << LayerMask.NameToLayer("StationaryEntities"));
        baseSpeed = speed;
        if (initialSpawnMaster != null)
        {
            initialSpawnMaster.spawnEnemies();
        }

    }

    private void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Pickup"))
        {
            handlePickup(collider.gameObject);
        }
        if (collider.gameObject.layer == LayerMask.NameToLayer("Fog"))
        {
            collider.gameObject.GetComponent<TilemapRenderer>().enabled = false;
        }
    }

    protected override void Update() {

        if (Time.timeScale == 0f)
        {
            return;
        }
        base.Update();
        if(isHit) {
            hitTimer += Time.deltaTime;
            if(hitTimer > hitTime) {
                isHit = false;
                hitTimer = 0f;
                speed = baseSpeed;
            }
        }

        if (isKnockback)
        {
            knockbackTimer += Time.deltaTime;
            if (knockbackTimer > knockbackTime)
            {
                isKnockback = false;
                knockbackTimer = 0f;
                speed = baseSpeed;
            }
        }

        Cursor.SetCursor(cursor.texture, new Vector2(8,8), CursorMode.Auto);

        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 position = new Vector2(transform.position.x, transform.position.y);
        Vector2 diff = (mousePos - position);
        Quaternion particleRotation = new Quaternion();
        Vector2 particlePosition = new Vector2();

        angle = (float)(getMouseAngle()/Math.PI * 180);
        particleRotation.eulerAngles = new Vector2(-angle,90);

        if((angle >= -22.5 && angle < 22.5) && diff.x > 0) {
            spriteRenderer.sprite = frontRight;
            particlePosition = new Vector2(transform.position.x + 1.25f, transform.position.y - 0.8f);
        } else if((angle >= 22.5 && angle < 67.5) && diff.x > 0) {
            spriteRenderer.sprite = diagUpRight;
            particlePosition = new Vector2(transform.position.x + 1.5f, transform.position.y + .1f);
        } else if((angle >= 67.5 && angle < 112.5) && diff.y > 0) {
            spriteRenderer.sprite = back;
            particlePosition = new Vector2(transform.position.x + .53f, transform.position.y + -.1f);
        } else if((angle >= 112.5 && angle < 157.5) && diff.y > 0) {
            spriteRenderer.sprite = diagUpLeft;
            particlePosition = new Vector2(transform.position.x - 1.5f, transform.position.y + .1f);
        } else if((angle >= 157.5 || angle < -157.5) && diff.x < 0) {
            spriteRenderer.sprite = frontLeft;
            particlePosition = new Vector2(transform.position.x - 1.25f, transform.position.y - 0.8f);
        } else if((angle >= -157.5 && angle < -112.5) && diff.x < 0) {
            spriteRenderer.sprite = diagDownLeft;
            particlePosition = new Vector2(transform.position.x - 1.27f, transform.position.y - 1.4f);
        } else if((angle >= -112.5 && angle < -67.5) && diff.y < 0) {
            spriteRenderer.sprite = frontDown;
            particlePosition = new Vector2(transform.position.x - 0.4f, transform.position.y - 0.55f);
        } else if((angle >= -67.5 && angle < -22.5) && diff.x > 0) {
            spriteRenderer.sprite = diagDownRight;
            particlePosition = new Vector2(transform.position.x + 1.27f, transform.position.y - 1.4f);
        }
        shotgunBlast.transform.rotation = particleRotation;
        shotgunBlast.transform.position = particlePosition;

        shotgunPellets.transform.rotation = particleRotation;
        shotgunPellets.transform.position = particlePosition;

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        if(!isKnockback)
            rb2d.velocity = new Vector2(moveHorizontal, moveVertical) * speed * Time.deltaTime;

        if(Input.GetMouseButtonDown(0) && shotReady) {
            shotgunBlast.Play();
            shotgunPellets.Play();
            shoot(Shells[(int)currentAmmo]);
            base.UseAmmo();

        }
    }

    private float getMouseAngle()
    {
        return Mathf.Atan2(this.mousePos.y - this.transform.position.y,
            this.mousePos.x - this.transform.position.x);
    }

    private void shoot(Shot shell)
    {
        int pellets = shell.pellets;
        float variance = shell.variance;
        float range = shell.range;
        int damage = shell.damage;

        SoundController.playShotgunShootSound();
        shotReady = false;
        float mouseAngle = getMouseAngle();
        Vector2[] pelletDirections = new Vector2[pellets];
        for(int i = 0; i < pelletDirections.Length; ++i)
        {
            float appliedAngle = mouseAngle - variance/2f + (variance/(float)pellets)*i;

            pelletDirections[i] = new Vector2(Mathf.Cos(appliedAngle), Mathf.Sin(appliedAngle));

            RaycastHit2D raycastResult =
                Physics2D.Raycast(this.transform.position, pelletDirections[i], range, interactsWithBullets);
            if(raycastResult.collider != null)
            {
                Debug.DrawLine(this.transform.position, raycastResult.point, Color.red, 0.1f);
                if(raycastResult.collider.gameObject.layer == LayerMask.NameToLayer("Obstacles"))
                {
                    GameObject tileMapGameObject = raycastResult.collider.gameObject;
                    tileMapGameObject.GetComponent<ObstacleController>().Break(raycastResult.point);
                }
                else if(raycastResult.collider.gameObject.layer == LayerMask.NameToLayer("Entities") ||
                        raycastResult.collider.gameObject.layer == LayerMask.NameToLayer("StationaryEntities"))
                {
                    EnemyHealth enemyHealth = raycastResult.collider.gameObject.GetComponent<EnemyHealth>();
                    enemyHealth.takeDamage(damage, BaseAttack.Element.Normal);
                }
            }
            else
            {
                Debug.DrawRay(this.transform.position, pelletDirections[i] * 100, Color.yellow, 0.1f);
            }

        }

    }

    public void takeDamage(float damage) {
        health -= damage;
        isHit = true;
        speed = baseSpeed / 4;
    }

    private void handlePickup(GameObject item) {
        item.GetComponent<ItemController>().Pickup();
        if(item.tag == "Heart") {
            if(health + heartHealth > MaxHealth)
                health = MaxHealth;
            else
                health += heartHealth;
            Destroy(item);
        }
        else if(item.tag == "Potion") {
            if(mana + potionMana > MaxMana)
                mana = MaxMana;
            else
                mana += potionMana;
            Destroy(item);
        }
        else if (item.tag == "BlueShell")
        {
            PickupAmmo(Ammo.BlueShell);
            Destroy(item);
        }
        else if (item.tag == "GreenShell")
        {
            PickupAmmo(Ammo.GreenShell);
            Destroy(item);
        }
    }

    public void onHitKnockback(float knockbackMagnitude, Vector3 hitDirection)
    {
        Vector2 unitVec = transform.position - hitDirection;
        unitVec.Normalize();
        rb2d.AddForce(unitVec * knockbackMagnitude);
        isKnockback = true;
    }

}
