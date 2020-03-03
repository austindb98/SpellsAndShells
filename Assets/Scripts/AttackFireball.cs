using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackFireball : BaseAttack
{
    private float dotDuration = 12f;
    private float dotFrequency = 3f;    // how often to inflict damage
    private float dotDamage = 2f;

    // Start is called before the first frame update
    void Start()
    {
        speed = 15;
        damage = 25;
        element = Element.Fire;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyController enemyController = collision.gameObject.GetComponent<EnemyController>();
        if(enemyController)
            enemyController.applyFireDotEffect(dotDuration, dotFrequency, dotDamage);
        base.OnTriggerEnter2D(collision);
    }
}
