using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BasePlayer : MonoBehaviour
{

    public Spell currentSpell;



    static readonly bool debug = false;
    static readonly float DebugSpeed = 30f;
    static readonly float ManaRegen = 3f;

    public float MaxHealth;
    public float MaxMana;

    public float health;
    public float mana;

    // Use to tell if shot has cooled down, will be true if it has cooled fully
    public bool shotReady;

    public bool skillsUpdated;

    public uint[] skillpoints = new uint[4];

    public int spellIndex = 0;

    protected void Start()
    {
        skillsUpdated = true;
        health = MaxHealth;
        mana = MaxMana;
        shotReady = true;
        if (SaveManager.currentSave != null)
        {
            skillpoints[0] = SaveManager.currentSave.unassigned;
            skillpoints[1] = SaveManager.currentSave.wind;
            skillpoints[2] = SaveManager.currentSave.ice;
            skillpoints[3] = SaveManager.currentSave.fire;
        } else
        {
            skillpoints[0] = 15;
        }
        
        if (debug)
        {
            health = MaxHealth * .5f;
            mana = MaxMana * .75f;
        }
    }

    public void UseMana(uint amt)
    {
        mana -= amt;
        if (mana < 0)
        {
            mana = 0;
        }
    }
    
    public void NextSpell()
    {
        spellIndex++;
        if (spellIndex >= 3)
        {
            spellIndex = 0;
        }
    }

    public void PreviousSpell()
    {
        spellIndex--;
        if (spellIndex < 0)
        {
            spellIndex = 2;
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
    
    protected virtual void Update()
    {
        if (debug)
        {
            DebugUpdate();
        }

        if (mana < MaxMana)
        {
            mana += ManaRegen * Time.deltaTime;
        }
    }

    private void OnDestroy()
    {
        if (SaveManager.currentSave == null)
        {
            Debug.LogError("Cannot save files, are you in a test scene?");
            return;
        }
        SaveManager.currentSave.unassigned = (byte)skillpoints[0];
        SaveManager.currentSave.wind = (byte)skillpoints[1];
        SaveManager.currentSave.ice = (byte)skillpoints[2];
        SaveManager.currentSave.fire = (byte)skillpoints[3];
        SaveManager.SaveAllFiles();
    }


}
