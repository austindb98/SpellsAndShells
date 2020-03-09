using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackIceRing : ChildAttack
{



    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyController enemyController = collision.gameObject.GetComponent<EnemyController>();
        if (enemyController)
        {
            enemyController.applyFrostSlowingEffect(AttackFrost.slowMag, AttackFrost.slowDur);
        }
    }
}
