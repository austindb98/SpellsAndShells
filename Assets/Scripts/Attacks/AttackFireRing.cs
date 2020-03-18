﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class AttackFireRing : MonoBehaviour
{
    public float Lifetime = 3f;

    public AttackFireball parent;

    private CircleCollider2D ring;

    bool attackHappened;

    // Start is called before the first frame update
    void Start()
    {
        ring = GetComponent<CircleCollider2D>();
        transform.rotation = Quaternion.Euler(0, 0, 0);
        Destroy(parent.gameObject, Lifetime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*private void LateUpdate()
    {
        if (attackHappened == true)
        {
            ring.enabled = false;
        }
    }*/

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyController enemyController = collision.gameObject.GetComponent<EnemyController>();
        if (enemyController && enemyController.gameObject.tag != "DestructibleSpell") {
            enemyController.applyFireDotEffect(AttackFireball.dotDuration, AttackFireball.dotFrequency, AttackFireball.dotDamage);
            enemyController.HandleMercyAttack(parent.damage, BaseAttack.Element.Fire);
            attackHappened = true;
        }
    }
}
