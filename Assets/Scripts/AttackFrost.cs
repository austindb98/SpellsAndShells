using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackFrost : BaseAttack
{
    private float frostDmg = 50f;

    private float slowMag = 0.6f;
    private float slowDur = 10f;

    // Start is called before the first frame update
    void Start()
    {
        speed = 8;
        damage = 20;
        element = Element.Ice;
    }

    protected override void OnTriggerEnter2D(Collider2D collider) {
        EnemyController enemyController = collider.gameObject.GetComponent<EnemyController>();
        enemyController.applyFrostSlowingEffect(slowMag, slowDur);
        base.OnTriggerEnter2D(collider);
    }


}
