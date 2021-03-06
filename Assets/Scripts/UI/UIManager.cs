﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    public GameObject skillUi;
    public GameObject pauseMenu;
    public GameObject deathMenu;
    public BasePlayer player;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Skills") && !pauseMenu.activeInHierarchy && !DialogScheduler.HasDialog())
        {

            if (skillUi.activeInHierarchy)
            {
                skillUi.SetActive(false);
                Time.timeScale = 1;
            } else
            {
                skillUi.SetActive(true);
                Time.timeScale = 0;
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            }

        }
        else if (Input.GetButtonDown("Pause") && !skillUi.activeInHierarchy && !DialogScheduler.HasDialog())
        {
            if (pauseMenu.activeInHierarchy)
            {
                pauseMenu.SetActive(false);
                Time.timeScale = 1;
            }
            else
            {
                pauseMenu.SetActive(true);
                Time.timeScale = 0;
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            }
        }
        else if (Input.GetButtonDown("Die"))
        {
            deathMenu.SetActive(true);
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }


        if (player.health <= 0)
        {
            deathMenu.SetActive(true);
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }

    }
}
