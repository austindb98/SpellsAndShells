using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HudKey : MonoBehaviour
{

    private Animator an;


    void Start()
    {
        an = GetComponent<Animator>();
    }

    public void Show()
    {
        an.SetBool("showing", true);
    }

    public void Hide()
    {
        an.SetBool("showing", false);
    }
    
}
