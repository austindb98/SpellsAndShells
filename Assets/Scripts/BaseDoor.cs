using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseDoor : MonoBehaviour
{

    private bool InDoorway;
    protected bool Locked = true;    
    protected SpriteRenderer sr;
    private AudioSource doorOpen;

    public DoorType doorType;
    public Sprite unlockedSprite;

    public enum DoorType
    {
        Scene,
        Wind,
        Fire,
        Ice
    }

    protected virtual void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        doorOpen = GetComponent<AudioSource>();
    }


    public abstract void HandleUnlocked(); // child will need to implement this specifically

    protected virtual void HandleLocked()
    {
        Locked = false;
        doorOpen.Play();
        sr.sprite = unlockedSprite;
    }

    protected virtual void Update()
    {
        if (InDoorway && Input.GetButtonDown("Interact"))
        {
            if (Locked)
            {
                HandleLocked();
            }
            else
            {
                HandleUnlocked();
            }
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        InDoorway = true;
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        InDoorway = false;
    }
}
