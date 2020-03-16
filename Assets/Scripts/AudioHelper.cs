using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioHelper : MonoBehaviour
{

    public VolumeController.Setting channel;

    private AudioSource src;
    // Start is called before the first frame update
    void Start()
    {

        src = GetComponent<AudioSource>();
        src.volume = VolumeController.GetPreferredVolume(channel);
    }
}
