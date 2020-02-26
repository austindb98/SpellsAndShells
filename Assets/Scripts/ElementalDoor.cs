using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementalDoor : BaseDoor
{
    public SpriteRenderer overlay;
    public Sprite overlayActive;
    public Sprite overlayInactive;

    public GameObject grate;


    protected override void HandleUnlocked()
    {
        // do nothing, all work is done already
    }

    protected override void HandleLocked()
    {
        if (!KeyManager.HasKeyType(doorType))
        {
            return;
            //TODO play locked sound
        }
        KeyManager.RemoveKeyType(doorType);
        base.HandleLocked();
        grate.SetActive(false);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        overlay.sprite = overlayActive;
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);
        if (Locked)
        {
            overlay.sprite = overlayInactive;
        }
        
    }
}
