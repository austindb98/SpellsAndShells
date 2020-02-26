using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementalDoor : BaseDoor
{
    public SpriteRenderer overlay;
    public Sprite overlayActive;
    public Sprite overlayInactive;

    protected override void HandleUnlocked()
    {
        // do nothing, all work is done already
    }

    protected override void HandleLocked()
    {
        base.HandleLocked();
        BoxCollider2D[] colliders = GetComponents<BoxCollider2D>();
        //colliders[0].enabled = false;
        sr.sortingLayerName = "Foreground";
        colliders[1].enabled = false;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        overlay.sprite = overlayActive;
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);
        overlay.sprite = overlayInactive;
    }
}
