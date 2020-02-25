using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class MenuController : MonoBehaviour
{
    // Start is called before the first frame update
    protected virtual void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadScene(string sceneName)
    {
        SoundController.playMenuChange();
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {

        Application.Quit();
    }
}
