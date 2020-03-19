﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHurricane : AttackGust
{
    public float stunTime = 2f;

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyController enemyController = collision.gameObject.GetComponent<EnemyController>();
        if (enemyController && enemyController.gameObject.tag != "DestructibleSpell")
            enemyController.ApplyStun(stunTime);
        base.OnTriggerEnter2DParent(collision); // skips to baseattack
    }
}
