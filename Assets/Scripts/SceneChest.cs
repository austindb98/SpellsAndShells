using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChest : MonoBehaviour
{

    private bool Interactable;

    static string chestText = "Seems locked. Breaking it might work";
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Interactable && Input.GetButtonDown("Interact") && !DialogScheduler.HasDialog())
        {
            DialogScheduler.addDialog(chestText, 1.2f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Interactable = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        Interactable = false;
    }
}
