﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathMenuController : PauseMenuController
{

    public BasePlayer player;

    private void Awake()
    {
        Time.timeScale = 0;
    }


    public void Restart()
    {
        SoundController.playMenuChange();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    

}
