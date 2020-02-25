using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class PauseMenuController : MenuController
{
    

    public Text levelText;

    protected override void Start()
    {
        base.Start();
        if (SaveManager.currentSave != null)
        {
            levelText.text = "LEVEL: " + SaveManager.currentSave.level;
        }
        
    }

    public void Resume()
    {
        SoundController.playMenuChange();
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }
}
