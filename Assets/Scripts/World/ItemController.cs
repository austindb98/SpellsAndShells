using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    private GameObject soundManager;
    public AudioClip pickupSound;
    // Start is called before the first frame update
    void Start()
    {
        soundManager = GameObject.Find("SoundManager");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Pickup() {
        soundManager.GetComponent<AudioSource>().PlayOneShot(pickupSound);
    }
}
