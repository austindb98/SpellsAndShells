using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneDoor : BaseDoor
{
    public int nextScene = 0;

    protected override void Start()
    {
        base.Start();
        doorType = BaseDoor.DoorType.Scene;
    }

    protected override void HandleUnlocked()
    {
        if (SaveManager.currentSave != null)
        {
            SaveManager.currentSave.level++;
            SaveManager.SaveAllFiles();
        }
        
        SceneManager.LoadScene(nextScene);
    }
}
