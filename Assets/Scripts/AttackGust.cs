using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackGust : BaseAttack
{
    private float knockbackMagnitude = 10f;
    // Start is called before the first frame update
    protected override void Start()
    {
        element = Element.Wind;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyController enemyController = collision.gameObject.GetComponent<EnemyController>();
        if(enemyController)
            enemyController.applyWindKnockbackEffect(knockbackMagnitude);
        base.OnTriggerEnter2D(collision);
    }
}
