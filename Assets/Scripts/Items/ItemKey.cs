using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemKey : ItemController
{
    public BaseDoor.DoorType doorType;

    public override void Pickup()
    {
        base.Pickup();
        if (doorType == BaseDoor.DoorType.Scene)
        {
            KeyManager.AddSceneKey();
        }
        else if (doorType == BaseDoor.DoorType.Wind)
        {
            KeyManager.AddWindKey();
        }
        else if (doorType == BaseDoor.DoorType.Ice)
        {
            KeyManager.AddIceKey();
        }
        else if (doorType == BaseDoor.DoorType.Fire)
        {
            KeyManager.AddFireKey();
        }
    }
}
