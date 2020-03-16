using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public static AudioClip shotgunDropSound;
    public static AudioClip potBreakSound;
    public static AudioClip menuChange;
    static AudioClip stoneDestroy;
    static AudioClip playerHurt;
    static AudioClip levelUp;
    static AudioClip shotgunShootSound;
    static AudioClip menuError;

    public AudioClip[] sfx = new AudioClip[7];

    private static AudioSource soundSource;

    public static float sfxVolume;
    public static float uiVolume;

    private static readonly float shotgunSfxMult = .5f;

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
            shotgunDropSound = sfx[6];
            menuError = sfx[7];
        }

        sfxVolume = 1f; // later will get from playerprefs
        uiVolume = 1f;
        OnVolumesUpdated();
    }

    public static void OnVolumesUpdated()
    {
        sfxVolume = PlayerPrefs.GetFloat(VolumeController.Setting.VolumeSfx.ToString(), 1);
        uiVolume = PlayerPrefs.GetFloat(VolumeController.Setting.VolumeUI.ToString(), 1);
        Debug.Log("sfx = " + sfxVolume + ", ui = " + uiVolume);
    }

    public static void playShotgunShootSound()
    {
        soundSource.PlayOneShot(shotgunShootSound, sfxVolume * shotgunSfxMult);
    }

    public static void playBreakSound(AudioClip breakSound)
    {
        soundSource.PlayOneShot(breakSound, sfxVolume);
    }

    public static void playDropSound()
    {
        soundSource.PlayOneShot(shotgunDropSound, sfxVolume);
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

    public static void PlayError()
    {
        soundSource.PlayOneShot(menuError, uiVolume);
    }
}
