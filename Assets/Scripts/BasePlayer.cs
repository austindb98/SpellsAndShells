using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePlayer : MonoBehaviour
{

    public enum Skill
    {
        Null, Ice, Forcefield, RingofFire, Drink, Firestorm, Whirlwind, Lightning
    }



    static readonly bool Debug = true;
    static readonly float DebugSpeed = 30f;

    public float MaxHealth;
    public float MaxMana;

    public float health;
    public float mana;

    public Skill skill1;
    public Skill skill2;
    public Skill skill3;

    public uint[] skillpoints = new uint[14];

    void Start()
    {
        health = MaxHealth;
        mana = MaxMana;
        if (Debug)
        {
            health = MaxHealth * .5f;
            mana = MaxMana * .75f;
        }
    }

    private void DebugUpdate()
    {
        mana += Time.deltaTime * DebugSpeed;
        if (mana > MaxMana)
        {
            mana = 0;
        }

        health += Time.deltaTime * DebugSpeed;
        if (health > MaxHealth)
        {
            health = 0;
        }
    }
    
    void Update()
    {
        if (Debug)
        {
            DebugUpdate();
        }
    }
}
