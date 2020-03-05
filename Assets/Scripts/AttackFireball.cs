using System;
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackFireball : BaseAttack
{
    private float dotDuration = 5f;
    private float dotFrequency = .5f;    // how often to inflict damage
    private float dotDamage = 2f;

    private float explosionRadius = 1f;
    public AudioClip explosionSound;

    public FireType fireType;

    public enum FireType
    {
        Fireball, Meteorite, MeteorShower, Armageddon
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        element = Element.Fire;

    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    protected void OnDisable() {
        TriggerExplosion(transform.position);
        Debug.Log("Trigger explosion at " + transform.position);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyController enemyController = collision.gameObject.GetComponent<EnemyController>();
        if(enemyController)
            enemyController.applyFireDotEffect(dotDuration, dotFrequency, dotDamage);
        if (fireType >= FireType.Meteorite)
        {
            Debug.Log("HIT at " + transform.position);

        }
        base.OnTriggerEnter2D(collision);
    }

    void TriggerExplosion(Vector2 center) {
        SoundController.PlaySound(explosionSound);
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(center, explosionRadius);
        for(int i = 0; i < hitColliders.Length; i++) {
            Debug.Log("Hit object " + hitColliders[i].gameObject.name + " at " + hitColliders[i].gameObject.transform.position);
            EnemyController enemyController = hitColliders[i].gameObject.GetComponent<EnemyController>();
            if(enemyController != null) {
                Debug.Log("DOT enemy at " + hitColliders[i].gameObject.transform.position);
                enemyController.applyFireDotEffect(dotDuration, dotFrequency, dotDamage);
            }
        }
    }
}
