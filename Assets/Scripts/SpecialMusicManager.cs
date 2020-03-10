using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class SpecialMusicManager : MusicManager
{
    public AudioClip specialSong;

    private bool activated;

    public GameObject destroyEndsSong;//like a boss for instance

    protected override void PlayNext()
    {
        if (activated)
        {
            src.PlayOneShot(specialSong);
        } else
        {
            base.PlayNext();
        }

    }

    protected override void Update()
    {
        if (activated && destroyEndsSong == null)
        {
            activated = false;
            src.Stop();
            PlayNext();
        } else
        {
            base.Update();
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (destroyEndsSong == null)
        {
            return;
        }
        GetComponent<BoxCollider2D>().enabled = false;
        activated = true;
        src.Stop();
        PlayNext();
    }
}
