using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public AudioClip shotgunShootSound;
    public AudioClip potBreakSound;

    private AudioSource soundSource;

    void Start()
    {
        soundSource = GetComponent<AudioSource>();
    }

    public void playShotgunShootSound()
    {
        soundSource.PlayOneShot(shotgunShootSound, 1.0f);
    }

    public void playPotBreakSound()
    {
        soundSource.PlayOneShot(potBreakSound, 1.0f);
    }
}
