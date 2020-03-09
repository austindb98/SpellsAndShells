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

    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    protected override void OnDeath()
    {
       
        if (fireType >= FireType.Meteorite)
        {

            Destroy(this);
        } else
        {
            base.OnDeath();
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyController enemyController = collision.gameObject.GetComponent<EnemyController>();
        if(enemyController)
            enemyController.applyFireDotEffect(dotDuration, dotFrequency, dotDamage);
        if (fireType >= FireType.Meteorite)
        {
            TriggerExplosion();

        }
        base.OnTriggerEnter2D(collision);
    }

    void TriggerExplosion() {
        
        //gameObject.SetActive(false);
        ringOfFire.SetActive(true);
    }
}
