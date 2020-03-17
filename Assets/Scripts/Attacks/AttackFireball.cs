using System;
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackFireball : BaseAttack
{
    public static float dotDuration = 5f;
    public static float dotFrequency = .5f;    // how often to inflict damage
    public static float dotDamage = 2f;
    

    public FireType fireType;

    public GameObject ringOfFire;

    public enum FireType
    {
        Fireball, Meteorite, MeteorShower, Armageddon
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        element = Element.Fire;
        base.Start();
    }



    public override void DoneMoving()
    {
        if (canMove == false)
        {
            return;
        }
        if (fireType >= FireType.Meteorite)
        {
            ringOfFire.SetActive(true);
            particles.Stop();
        }
        base.DoneMoving();
    }

    public override void OnDeath()
    {
       
        if (fireType >= FireType.Meteorite)
        {
            DoneMoving();
        } else
        {
            base.OnDeath();
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        
        EnemyController enemyController = collision.gameObject.GetComponent<EnemyController>();
        if(enemyController && enemyController.gameObject.tag != "DestructibleSpell")
            enemyController.applyFireDotEffect(dotDuration, dotFrequency, dotDamage);

        base.OnTriggerEnter2D(collision);
        
       
    }
    
}
