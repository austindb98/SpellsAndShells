using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public static AudioClip shotgunShootSound;
    public static AudioClip potBreakSound;
    public static AudioClip menuChange;
    static AudioClip stoneDestroy;
    static AudioClip playerHurt;
    static AudioClip levelUp;

    public AudioClip[] sfx = new AudioClip[6];

    private static AudioSource soundSource;

    private static float sfxVolume;
    private static float uiVolume;

    private static readonly float shotgunSfxMult = .125f;

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
            playerHurt = sfx[4];
            levelUp = sfx[5];
        }

        sfxVolume = 1f; // later will get from playerprefs
        uiVolume = 1f;
    }

    public static void playShotgunShootSound()
    {
        soundSource.PlayOneShot(shotgunShootSound, sfxVolume * shotgunSfxMult);
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

    public static void PlayPlayerHurt()
    {
        soundSource.PlayOneShot(playerHurt, sfxVolume);
    }

    public static void PlayLevelUp()
    {
        soundSource.PlayOneShot(levelUp, sfxVolume);
    }

    public static void PlaySound(AudioClip clip)
    {
        soundSource.PlayOneShot(clip, sfxVolume);
    }
}
