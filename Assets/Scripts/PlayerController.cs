﻿using System.Collections;
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
        new Shot(1,0f,30f,85),
        new Shot(25,1f,15f,3),
        new Shot(1,0f,30f,75),
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
    private float knockbackTime = 0.5f;
    private bool isKnockback = false;

    public LayerMask wallLayer, obstacleLayer, fogLayer;
    private LayerMask interactsWithBullets;
    private Vector2 mousePos;
    private Vector2 rayPosition;
    private float heartHealth = 30f;
    private float potionMana = 30f;

    static readonly float slowdownMult = .35f;

    public SpawnMaster initialSpawnMaster; // this will not be necessary in the final design

    public bool canMove = true;

    public bool haungsMode = false;
    Color haungsRendererColor = new Color(255, 172, 0);

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
    }

    protected override void Update() {
        if (Time.timeScale == 0f || !canMove)
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

        angle = (float)(getMouseAngle(transform.position)/Math.PI * 180);

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

        float particleAngle = (float) (getMouseAngle(particlePosition)/Math.PI * 180);
        particleRotation.eulerAngles = new Vector2(-particleAngle,90);

        shotgunBlast.transform.rotation = particleRotation;
        shotgunBlast.transform.position = particlePosition;

        shotgunPellets.transform.rotation = particleRotation;
        shotgunPellets.transform.position = particlePosition;
        var pelletShape = shotgunPellets.shape;
        pelletShape.angle = Shells[(int) base.currentAmmo].variance * (float) (180/Math.PI)/2;

        rayPosition = new Vector2(transform.position.x, particlePosition.y);

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector2 moveVector = new Vector2(moveHorizontal, moveVertical);
        if(moveVector.magnitude > 1) {
            moveVector = moveVector.normalized;
        }
        if(!isKnockback)
            rb2d.velocity =  moveVector * speed * Time.deltaTime;

        if(Input.GetMouseButtonDown(0) && shotReady) {
            shotgunBlast.Play();
            shotgunPellets.Play();
            shoot(Shells[(int)currentAmmo]);
            base.UseAmmo();
        }

        if(Input.GetKeyDown("r") && base.AmmoCount >= 1) {
            SoundController.playDropSound();
            base.currentAmmo = BasePlayer.Ammo.RedShell;
            base.AmmoCount = -1;
        }

        manageHaungsMode();
    }

    private float getMouseAngle(Vector2 position)
    {
        return Mathf.Atan2(this.mousePos.y - position.y,
            this.mousePos.x - position.x);
    }

    private void shoot(Shot shell)
    {
        int pellets = shell.pellets;
        float variance = shell.variance;
        float range = shell.range;
        int damage = shell.damage;

        SoundController.playShotgunShootSound();
        shotReady = false;
        float mouseAngle = getMouseAngle(rayPosition);
        Vector2[] pelletDirections = new Vector2[pellets];
        for(int i = 0; i < pelletDirections.Length; ++i)
        {
            float appliedAngle = mouseAngle - variance/2f + (variance/(float)pellets)*i;

            pelletDirections[i] = new Vector2(Mathf.Cos(appliedAngle), Mathf.Sin(appliedAngle));

            RaycastHit2D raycastResult =
                Physics2D.Raycast(rayPosition, pelletDirections[i], range, interactsWithBullets);
            if(raycastResult.collider != null)
            {
                Debug.DrawLine(rayPosition, raycastResult.point, Color.red, 0.1f);
                if(raycastResult.collider.gameObject.layer == LayerMask.NameToLayer("Obstacles"))
                {
                    GameObject tileMapGameObject = raycastResult.collider.gameObject;
                    tileMapGameObject.GetComponent<ObstacleController>().PlayerBreak(raycastResult.point);
                }
                else if(raycastResult.collider.gameObject.layer == LayerMask.NameToLayer("Entities") ||
                        raycastResult.collider.gameObject.layer == LayerMask.NameToLayer("StationaryEntities"))
                {
                    if(raycastResult.collider.gameObject.tag == "mendohl") {
                        GameObject.FindWithTag("BossRoom").GetComponent<FinalSceneController>().handleShootMendohl();
                    }
                    else {
                        EnemyController enemyController = raycastResult.collider.gameObject.GetComponent<EnemyController>();
                        enemyController.handleShotgunAttack(shell.damage);
                    }
                }
            }
            else
            {
                Debug.DrawRay(rayPosition, pelletDirections[i] * 100, Color.yellow, 0.1f);
            }

        }

    }

    public void takeDamage(float damage) {
        if (base.MarkInvulnerable())
        {
            return;
        }
        health -= damage;
        isHit = true;
        speed = baseSpeed * slowdownMult;
        SoundController.PlayPlayerHurt();
    }

    private void handlePickup(GameObject item) {
        if(item.tag != "GoldShell")
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
        else if (item.tag == "GoldShell")
        {
            PickupAmmo(Ammo.GoldShell);
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

    private void manageHaungsMode()
    {
        if (Input.GetKeyDown("h"))
        {
            haungsMode = !haungsMode;
            if (haungsMode)
            {
                spriteRenderer.color = haungsRendererColor;
            }
            else
            {
                spriteRenderer.color = Color.white;
            }
        }

        if (haungsMode)
        {
            base.health = base.MaxHealth;
            base.mana = base.MaxMana;

            if (Input.GetKeyDown("n") )
            {
                haungsSkipToNextLevel();
            }

            if (Input.GetKeyDown("m"))
            {
                haungsAddSkillPoints();
            }
        }

       
    }

    private void haungsSkipToNextLevel()
    {
        GameObject[] lookingForSceneDoor = GameObject.FindGameObjectsWithTag("SceneDoor");
        GameObject sceneDoor;
        if (lookingForSceneDoor.Length > 0)
        {
            sceneDoor = lookingForSceneDoor[0];
            sceneDoor.GetComponent<SceneDoor>().HandleUnlocked();
        }
    }

    private void haungsAddSkillPoints()
    {
        base.skillpoints[0] += 1;
    }
}
