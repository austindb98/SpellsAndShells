using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{

    public static float musicVolume = .2f;

    public AudioClip[] songs;

    private int currentIndex;
    protected AudioSource src;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        src = GetComponent<AudioSource>();
        src.volume = musicVolume;
        OnVolumeUpdated();
        PlayNext();
    }

    public static void OnVolumeUpdated()
    {
        musicVolume = PlayerPrefs.GetFloat(VolumeController.Setting.VolumeMusic.ToString(), 1);
    }

    protected virtual void PlayNext()
    {
        int next;
        do
        {
            next = Random.Range(0, songs.Length);
        } while (next == currentIndex);
        PlayIndex(next);

    }

    private void PlayIndex(int index)
    {
        currentIndex = index;
        src.PlayOneShot(songs[currentIndex], musicVolume);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (!src.isPlaying)
        {
            PlayNext();
        }
    }
}
