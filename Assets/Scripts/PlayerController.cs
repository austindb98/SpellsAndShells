using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : BasePlayer {

    public float speed;
    public double angle;
    public Sprite frontLeft, frontRight, frontDown, back;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb2d;

    void Start() {
        rb2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update() {
        Vector2 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 position = new Vector2(transform.position.x, transform.position.y);
        Vector2 diff = (position - mouse);
        angle = Math.Atan(diff.y / diff.x) * 180/Math.PI;

        if((angle >= -45 && angle < 45) && diff.x < 0) {
            spriteRenderer.sprite = frontRight;
        } else if((angle >= -45 && angle < 45) && diff.x > 0) {
            spriteRenderer.sprite = frontLeft;

        } else if((angle >= 45 || angle < -45) && diff.y > 0) {
            spriteRenderer.sprite = frontDown;
        } else if((angle >= 45 || angle < -45) && diff.y < 0) {
            spriteRenderer.sprite = back;
        }

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        transform.Translate(new Vector2(moveHorizontal, moveVertical) * speed * Time.deltaTime);

    }
}
