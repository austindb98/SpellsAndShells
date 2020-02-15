using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    public GameObject skillUi;
    public GameObject pauseMenu;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Skills"))
        {
            if (skillUi.activeInHierarchy)
            {
                skillUi.SetActive(false);
                Time.timeScale = 1;
            } else
            {
                skillUi.SetActive(true);
                Time.timeScale = 0;
            }
            
        }
        else if (Input.GetButtonDown("Pause"))
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
            }
        }
    }
}
