using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{

    public static readonly float musicVolume = .2f;

    public AudioClip[] songs;

    private int currentIndex;
    private AudioSource src;
    // Start is called before the first frame update
    void Start()
    {
        src = GetComponent<AudioSource>();
        src.volume = musicVolume;
        PlayNext();
    }

    private void PlayNext()
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
        src.PlayOneShot(songs[currentIndex]);
    }

    // Update is called once per frame
    void Update()
    {
        if (!src.isPlaying)
        {
            PlayNext();
        }
    }
}
