using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class MenuController : MonoBehaviour
{
    private AudioSource menuChange;
    // Start is called before the first frame update
    void Start()
    {
        menuChange = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadScene(string scneneName)
    {
        menuChange.Play();
        SceneManager.LoadScene(scneneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
