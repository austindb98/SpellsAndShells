using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyManager : MonoBehaviour
{

    public HudKey sceneKey;
    public HudKey windKey;
    public HudKey iceKey;
    public HudKey fireKey;

    private bool hasScene;
    private bool hasWind;
    private bool hasIce;
    private bool hasFire;
    

    private static KeyManager Instance;

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public static bool HasSceneKey()
    {
        return Instance.sceneKey;
    }

    public static bool HasWindKey()
    {
        return Instance.windKey;
    }

    public static bool HasIceKey()
    {
        return Instance.iceKey;
    }

    public static bool HasFireKey()
    {
        return Instance.fireKey;
    }

    public static void AddSceneKey()
    {
        AddKey(Instance.sceneKey, ref Instance.hasScene);
    }

    public static void AddWindKey()
    {
        AddKey(Instance.windKey, ref Instance.hasWind);
    }

    public static void AddIceKey()
    {
        AddKey(Instance.iceKey, ref Instance.hasIce);
    }

    public static void AddFireKey()
    {
        AddKey(Instance.fireKey, ref Instance.hasFire);
    }

    public static void RemoveSceneKey()
    {
        RemoveKey(Instance.sceneKey, ref Instance.hasScene);
    }

    public static void RemoveWindKey()
    {
        RemoveKey(Instance.windKey, ref Instance.hasWind);
    }

    public static void RemoveIceKey()
    {
        RemoveKey(Instance.iceKey, ref Instance.hasIce);
    }

    public static void RemoveFireKey()
    {
        RemoveKey(Instance.fireKey, ref Instance.hasFire);
    }

    private static void AddKey(HudKey key, ref bool hasToSet)
    {
        key.Show();
        hasToSet = true;
    }

    private static void RemoveKey(HudKey key, ref bool hasToSet)
    {
        key.Hide();
        hasToSet = false;
    }
}
