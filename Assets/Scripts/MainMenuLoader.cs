using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MainMenuLoader : MonoBehaviour
{
    
    public Button startButton;

    
    // Start is called before the first frame update
    void Start()
    {
        

        SaveManager.LoadSaveFiles();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
