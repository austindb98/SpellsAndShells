using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartGameLoader : MonoBehaviour
{

    public Text slot1;
    public Text slot2;
    public Text slot3;
    public Button button1;
    public Button button2;
    public Button button3;

    public GameObject delete1;
    public GameObject delete2;
    public GameObject delete3;
    public GameObject play1;
    public GameObject play2;
    public GameObject play3;

    public InputField nameField;

    // Start is called before the first frame update
    void Start()
    {
        if (SaveManager.save1 != null)
        {
            slot1.text = SaveManager.save1.name;
            delete1.SetActive(true);
            play1.SetActive(true);
            button1.interactable = false;
        }
        if (SaveManager.save2 != null)
        {
            slot2.text = SaveManager.save2.name;
            delete2.SetActive(true);
            play2.SetActive(true);
            button2.interactable = false;
        }
        if (SaveManager.save3 != null)
        {
            slot3.text = SaveManager.save3.name;
            delete3.SetActive(true);
            play3.SetActive(true);
            button3.interactable = false;
        }

        nameField.text = SaveManager.GenerateName();
    }
    

    public void FillSlot1()
    {
        SaveManager.CreateSave1(nameField.text);
        PlaySave(SaveManager.save1);
    }

    public void FillSlot2()
    {
        SaveManager.CreateSave2(nameField.text);
        PlaySave(SaveManager.save2);
    }

    public void FillSlot3()
    {
        SaveManager.CreateSave3(nameField.text);
        PlaySave(SaveManager.save3);
    }

    public void Play1()
    {
        PlaySave(SaveManager.save1);
    }

    public void Play2()
    {
        PlaySave(SaveManager.save2);
    }

    public void Play3()
    {
        PlaySave(SaveManager.save3);
    }


    private void PlaySave(SaveManager.Save save)
    {
        SaveManager.currentSave = save;
        //SaveManager.SaveAllFiles();
        SceneManager.LoadScene(SaveManager.currentSave.level);
    }

    public void Delete1()
    {
        DeleteSave(slot1, SaveManager.save1, button1, play1, delete1);
    }

    public void Delete2()
    {
        DeleteSave(slot2, SaveManager.save2, button2, play2, delete2);
    }

    public void Delete3()
    {
        DeleteSave(slot3, SaveManager.save3, button3, play3, delete3);
    }

    private void DeleteSave(Text slot, SaveManager.Save save, Button button, GameObject play, GameObject delete)
    {
        slot.text = "Empty";
        save = null;
        button.interactable = true;
        play.SetActive(false);
        delete.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
