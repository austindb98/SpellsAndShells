using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackFireball : BaseAttack
{
    public float dotDuration = 12f;
    public float dotFrequency = 3f;    // how often to inflict damage
    public float dotDamage = 2f;

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
    

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyController enemyController = collision.gameObject.GetComponent<EnemyController>();
        if(enemyController)
            enemyController.applyFireDotEffect(dotDuration, dotFrequency, dotDamage);
        if (fireType >= FireType.Meteorite)
        {
            //spawn fire articles
        }
        base.OnTriggerEnter2D(collision);
    }
}
