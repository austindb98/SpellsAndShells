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

    public static bool HasKeyType(BaseDoor.DoorType type)
    {
        if (type == BaseDoor.DoorType.Scene)
        {
            return HasSceneKey();
        }
        else if (type == BaseDoor.DoorType.Wind)
        {
            return HasWindKey();
        }
        else if (type == BaseDoor.DoorType.Ice)
        {
            return HasIceKey();
        }
        else if (type == BaseDoor.DoorType.Fire)
        {
            return HasFireKey();
        }
        return false;
    }

    public static bool HasSceneKey()
    {
        return Instance.hasScene;
    }

    public static bool HasWindKey()
    {
        return Instance.hasWind;
    }

    public static bool HasIceKey()
    {
        return Instance.hasIce;
    }

    public static bool HasFireKey()
    {
        return Instance.hasFire;
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

    public static void RemoveKeyType(BaseDoor.DoorType type)
    {
        if (type == BaseDoor.DoorType.Scene)
        {
            RemoveSceneKey();
        }
        else if (type == BaseDoor.DoorType.Wind)
        {
            RemoveWindKey();
        }
        else if (type == BaseDoor.DoorType.Ice)
        {
            RemoveIceKey();
        }
        else if (type == BaseDoor.DoorType.Fire)
        {
            RemoveFireKey();
        }
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
