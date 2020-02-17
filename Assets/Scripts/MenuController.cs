using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class MenuController : MonoBehaviour
{
    public SoundController soundManager;
    // Start is called before the first frame update
    protected void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadScene(string sceneName)
    {
        soundManager.playMenuChange();
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {

        Application.Quit();
    }
}
