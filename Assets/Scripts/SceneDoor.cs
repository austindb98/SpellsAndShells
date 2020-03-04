using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneDoor : BaseDoor
{
    public int nextScene = 0;
    public GameObject skillPointPrefab;

    protected override void Start()
    {
        base.Start();
        doorType = BaseDoor.DoorType.Scene;
    }

    protected override void HandleLocked()
    {
        if (!KeyManager.HasSceneKey())
        {
            return;
        }
        dropSkillPoint();
        KeyManager.RemoveSceneKey();
        base.HandleLocked();
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
    private void dropSkillPoint()
    {
        if(skillPointPrefab != null)
        {
            GameObject skillPoint = Instantiate(skillPointPrefab);
            Vector3 position = transform.position;
            position.y = position.y - 10;
            skillPoint.transform.position = position;
        }
    }
}
