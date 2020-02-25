using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public static AudioClip shotgunShootSound;
    public static AudioClip potBreakSound;
    public static AudioClip menuChange;
    static AudioClip stoneDestroy;

    public AudioClip[] sfx = new AudioClip[4];

    private static AudioSource soundSource;

    private static float sfxVolume;
    private static float uiVolume;

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
            stoneDestroy = sfx[3];
        }

        sfxVolume = 1f; // later will get from playerprefs
        uiVolume = 1f;
    }

    public static void playShotgunShootSound()
    {
        soundSource.PlayOneShot(shotgunShootSound, sfxVolume);
    }

    public static void playBreakSound(AudioClip breakSound)
    {
        soundSource.PlayOneShot(breakSound, sfxVolume);
    }

    public static void playMenuChange()
    {
        soundSource.PlayOneShot(menuChange, uiVolume);
    }

    public static void playStoneDestroy()
    {
        soundSource.PlayOneShot(stoneDestroy, sfxVolume);
    }
}
