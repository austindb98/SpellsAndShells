using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;

public class PlayerController : BasePlayer {

    public float speed = 100f;
    public double angle;
    public Sprite frontLeft, diagUpLeft, back, diagUpRight, frontRight, diagDownRight, frontDown, diagDownLeft;
    public ParticleSystem shotgunBlast, shotgunPellets;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb2d;
    private bool isHit = false;
    private float hitTimer = 0f;
    private float hitTime = 0.5f;

    public GameObject soundManager;
    public LayerMask wallLayer, obstacleLayer;
    private LayerMask interactsWithBullets;
    private int numberPellets = 10;
    private float pelletAngleVariance = 0.05f; // in radians
    private float mouseAngle;
    private Vector2 mousePos;

    void Start() {
        base.Start();
        rb2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        interactsWithBullets = wallLayer | obstacleLayer;
    }

    protected override void Update() {
        if (Time.timeScale == 0f)
        {
            return;
        }
        base.Update();
        if(isHit) {
            hitTimer += Time.deltaTime;
            print(hitTimer);
            if(hitTimer > hitTime) {
                print("no longer stunned");
                isHit = false;
                hitTimer = 0f;
                speed = 100f;
            }
        }
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 position = new Vector2(transform.position.x, transform.position.y);
        Vector2 diff = (mousePos - position);
        Quaternion particleRotation = new Quaternion();
        Vector2 particlePosition = new Vector2();
        angle = Math.Atan(diff.y / diff.x) * 180/Math.PI;

        if((angle >= -22.5 && angle < 22.5) && diff.x > 0) {
            spriteRenderer.sprite = frontRight;
            particleRotation.eulerAngles = new Vector2(0,90);
            particlePosition = new Vector2(transform.position.x + 1.25f, transform.position.y - 0.8f);
        } else if((angle >= 22.5 && angle < 67.5) && diff.x > 0) {
            spriteRenderer.sprite = diagUpRight;
            particleRotation.eulerAngles = new Vector2(-30,90);
            particlePosition = new Vector2(transform.position.x + 1.5f, transform.position.y + .1f);
        } else if((angle >= 67.5 || angle < -67.5) && diff.y > 0) {
            spriteRenderer.sprite = back;
            particleRotation.eulerAngles = new Vector2(-90,90);
            particlePosition = new Vector2(transform.position.x + .53f, transform.position.y + -.1f);
        } else if((angle >= -67.5 && angle < -22.5) && diff.x < 0) {
            spriteRenderer.sprite = diagUpLeft;
            particleRotation.eulerAngles = new Vector2(-150,90);
            particlePosition = new Vector2(transform.position.x - 1.5f, transform.position.y + .1f);
        } else if((angle >= -22.5 && angle < 22.5) && diff.x < 0) {
            spriteRenderer.sprite = frontLeft;
            particleRotation.eulerAngles = new Vector2(-180,90);
            particlePosition = new Vector2(transform.position.x - 1.25f, transform.position.y - 0.8f);
        } else if((angle >= 22.5 && angle < 67.5) && diff.x < 0) {
            spriteRenderer.sprite = diagDownLeft;
            particleRotation.eulerAngles = new Vector2(-210,90);
            particlePosition = new Vector2(transform.position.x - 1.27f, transform.position.y - 1.4f);
        } else if((angle >= 67.5 || angle < -67.5) && diff.y < 0) {
            spriteRenderer.sprite = frontDown;
            particleRotation.eulerAngles = new Vector2(-270,90);
            particlePosition = new Vector2(transform.position.x - 0.4f, transform.position.y - 0.55f);
        } else if((angle >= -67.5 && angle < -22.5) && diff.x > 0) {
            spriteRenderer.sprite = diagDownRight;
            particleRotation.eulerAngles = new Vector2(-330,90);
            particlePosition = new Vector2(transform.position.x + 1.27f, transform.position.y - 1.4f);
        }
        shotgunBlast.transform.rotation = particleRotation;
        shotgunBlast.transform.position = particlePosition;

        shotgunPellets.transform.rotation = particleRotation;
        shotgunPellets.transform.position = particlePosition;

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        rb2d.velocity = new Vector2(moveHorizontal, moveVertical) * speed * Time.deltaTime;

        if(Input.GetMouseButtonDown(0) && shotReady) {
            Debug.Log("Boom");
            shotgunBlast.Play();
            shotgunPellets.Play();
            shoot();
        }
    }

    private float getMouseAngle()
    {
        return Mathf.Atan2(this.mousePos.y - this.transform.position.y,
            this.mousePos.x - this.transform.position.x);
    }

    private void shoot()
    {
        soundManager.GetComponent<SoundController>().playShotgunShootSound();
        shotReady = false;
        mouseAngle = getMouseAngle();
        Vector2[] pelletDirections = new Vector2[10];
        for(int i = 0; i < pelletDirections.Length; ++i)
        {
            float appliedAngle = mouseAngle + pelletAngleVariance * (i / 2);
            if (i % 2 == 0)
            {
                appliedAngle = mouseAngle - pelletAngleVariance * (i / 2);
            }

            pelletDirections[i] = new Vector2(Mathf.Cos(appliedAngle), Mathf.Sin(appliedAngle));

            RaycastHit2D raycastResult =
                Physics2D.Raycast(this.transform.position, pelletDirections[i], Mathf.Infinity, interactsWithBullets);
            if(raycastResult.collider != null)
            {
                Debug.DrawLine(this.transform.position, raycastResult.point, Color.red, 0.1f);
                if(raycastResult.collider.gameObject.layer == LayerMask.NameToLayer("Obstacles"))
                {
                    GameObject tileMapGameObject = raycastResult.collider.gameObject;
                    tileMapGameObject.GetComponent<ObstacleController>().Break(raycastResult.point);
                }
            }
            else
            {
                Debug.DrawRay(this.transform.position, pelletDirections[i] * 100, Color.yellow, 0.1f);
            }

        }

    }

    public void takeDamage(float damage) {
        isHit = true;
        speed = 8f;
    }
}
