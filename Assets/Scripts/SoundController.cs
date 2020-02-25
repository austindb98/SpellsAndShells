using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public static AudioClip shotgunShootSound;
    public static AudioClip potBreakSound;
    public static AudioClip menuChange;

    public AudioClip[] sfx = new AudioClip[3];

    private static AudioSource soundSource;

    void Start()
    {
        if (soundSource == null)
        {
            soundSource = GetComponent<AudioSource>();
        }
        if (shotgunShootSound == null)
        {
            shotgunShootSound = sfx[0];
            potBreakSound = sfx[1];
            menuChange = sfx[2];
        }
    }

    public static void playShotgunShootSound()
    {
        soundSource.PlayOneShot(shotgunShootSound, 1.0f);
    }

    public static void playPotBreakSound()
    {
        soundSource.PlayOneShot(potBreakSound, 1.0f);
    }

    public static void playMenuChange()
    {
        soundSource.PlayOneShot(menuChange, 1.0f);
    }
}
