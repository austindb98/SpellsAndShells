using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneDoor : BaseDoor
{
    public int nextScene = 0;
    public GameObject skillPointPrefab;
    public BasePlayer player;

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

    public override void HandleUnlocked()
    {
        if (SaveManager.currentSave != null)
        {
            player.OnLevelCompleted();
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
