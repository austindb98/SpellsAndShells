using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackGust : BaseAttack
{
    private float knockbackMagnitude = 10f;
    // Start is called before the first frame update
    void Start()
    {
        speed = 14;
        damage = 15;
        element = Element.Wind;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyController enemyController = collision.gameObject.GetComponent<EnemyController>();
        enemyController.applyWindKnockbackEffect(knockbackMagnitude);
        base.OnTriggerEnter2D(collision);
    }
}
