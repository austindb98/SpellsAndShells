using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (BasePlayer))]
public class MagicController : MonoBehaviour
{

    static readonly float MouseScrollSensitivity = .001f;
    static readonly float NumberOfScrolls = 2;

    private int mouseScrolls;

    private BasePlayer player;
    private AudioSource castSound;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<BasePlayer>();
        castSound = GetComponent<AudioSource>();
    }
    
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            castSound.Play();
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            mouseScrolls++;
            if (mouseScrolls >= NumberOfScrolls)
            {
                player.NextSpell();
                mouseScrolls = 0;
            }
            
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            
            mouseScrolls--;
            if (mouseScrolls <= -NumberOfScrolls)
            {
                player.PreviousSpell();
                mouseScrolls = 0;
            }
        }
    }
}
