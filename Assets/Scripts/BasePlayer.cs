using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BasePlayer : MonoBehaviour
{

    public enum Skill
    {
        Wind, Ice, Fire
    }



    static readonly bool debug = false;
    static readonly float DebugSpeed = 30f;

    public float MaxHealth;
    public float MaxMana;

    public float health;
    public float mana;

    // Use to tell if shot has cooled down, will be true if it has cooled fully
    public bool shotReady;

    public uint[] skillpoints = new uint[4];

    protected void Start()
    {
        health = MaxHealth;
        mana = MaxMana;
        if (SaveManager.currentSave != null)
        {
            skillpoints[0] = SaveManager.currentSave.unassigned;
            skillpoints[1] = SaveManager.currentSave.wind;
            skillpoints[2] = SaveManager.currentSave.ice;
            skillpoints[3] = SaveManager.currentSave.fire;
        }
        
        Debug.Log("unassigned from save:" + skillpoints[0]);
        if (debug)
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
        if (debug)
        {
            DebugUpdate();
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
