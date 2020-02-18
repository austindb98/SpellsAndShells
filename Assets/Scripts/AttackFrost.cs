using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackFrost : BaseAttack
{
    private float frostDmg = 50f;
    // Start is called before the first frame update
    void Start()
    {
        speed = 8;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        base.DefaultOnCollision();
        if(collision.gameObject.layer == LayerMask.NameToLayer("Entities")) {
            EnemyHealth enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();
            enemyHealth.takeDamage(frostDmg);
        }
    }
}
