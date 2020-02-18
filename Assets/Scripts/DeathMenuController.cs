using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathMenuController : PauseMenuController
{

    private void Awake()
    {
        Time.timeScale = 0;
    }


    public void Restart()
    {
        soundManager.playMenuChange();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
}
