﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;

public class PlayerController : BasePlayer {

    public float speed;
    public double angle;
    public Sprite frontLeft, frontRight, frontDown, back;
    public ParticleSystem shotgunBlast;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb2d;

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

    

    void Update() {
        if (Time.timeScale == 0)
        {
            return;
        }
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 position = new Vector2(transform.position.x, transform.position.y);
        Vector2 diff = (mousePos - position);
        angle = Math.Atan(diff.y / diff.x) * 180/Math.PI;

        if((angle >= -45 && angle < 45) && diff.x > 0) {
            spriteRenderer.sprite = frontRight;
            shotgunBlast.transform.rotation = new Quaternion(90,0,90,0);
            shotgunBlast.transform.position = new Vector2(transform.position.x + 1.25f, transform.position.y - 0.8f);
        } else if((angle >= -45 && angle < 45) && diff.x < 0) {
            spriteRenderer.sprite = frontLeft;
            shotgunBlast.transform.rotation = new Quaternion(90,0,-90,0);
            shotgunBlast.transform.position = new Vector2(transform.position.x - 1.25f, transform.position.y - 0.8f);
        } else if((angle >= 45 || angle < -45) && diff.y < 0) {
            spriteRenderer.sprite = frontDown;
            shotgunBlast.transform.rotation = new Quaternion(0,-90,90,0);
            shotgunBlast.transform.position = new Vector2(transform.position.x - 0.4f, transform.position.y - 0.55f);
        } else if((angle >= 45 || angle < -45) && diff.y > 0) {
            spriteRenderer.sprite = back;
            shotgunBlast.transform.rotation = new Quaternion(0,-90,-90,0);
        }

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        //rb2d.velocity = new Vector2(moveHorizontal, moveVertical) * speed * Time.deltaTime;
        transform.Translate(new Vector2(moveHorizontal, moveVertical) * speed * Time.deltaTime);

        if(Input.GetMouseButtonDown(0)) {
            Debug.Log("Boom");
            shotgunBlast.Play();
            shoot();
        }
        if (Input.GetMouseButton(1))
        {
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
                    Tilemap map = tileMapGameObject.GetComponent<Tilemap>();
                    var tilePos = tileMapGameObject.GetComponent<GridLayout>().WorldToCell(raycastResult.point);
                    map.SetTile(tilePos, null);
                    soundManager.GetComponent<SoundController>().playPotBreakSound();
                }
            }
            else
            {
                Debug.DrawRay(this.transform.position, pelletDirections[i] * 100, Color.yellow, 0.1f);
            }
            
        }
        
    }
}
