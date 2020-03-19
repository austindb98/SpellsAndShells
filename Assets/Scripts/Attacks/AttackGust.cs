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
        base.Start();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyController enemyController = collision.gameObject.GetComponent<EnemyController>();
        if(enemyController && enemyController.gameObject.tag != "DestructibleSpell")
            enemyController.applyWindKnockbackEffect(knockbackMagnitude);
        base.OnTriggerEnter2D(collision);
    }

    protected void OnTriggerEnter2DParent(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }
}
