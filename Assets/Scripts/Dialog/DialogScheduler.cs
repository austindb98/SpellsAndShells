﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

class ManagerObject
{
    
}

class Prefab : ManagerObject
{
    public GameObject prefab;
    public Vector3 position;
    public Prefab(GameObject prefab, Vector3 position)
    {
        this.prefab = prefab;
        this.position = position;
    }
}

class Dialog : ManagerObject
{
    public string text;
    public Color color;
    public Dialog(string text, Color color)
    {
        this.text = text;
        this.color = color;
    }
}

public class DialogScheduler : MonoBehaviour
{
    private static Transform dialogBox;
    private static Text dialogText;
    private static Queue<ManagerObject> objects;
    private static Dialog currentDialog;
    private static Color blackColor = Color.black;

    public string OnStartDialog;
    // Start is called before the first frame update
    void Start()
    {
        objects = new Queue<ManagerObject>();
        dialogBox = transform.Find("DialogBox");
        dialogText = dialogBox.GetComponentInChildren<Text>();
        dialogBox.gameObject.SetActive(false);
        if (OnStartDialog != null && OnStartDialog != "")
        {
            dialogBox.gameObject.SetActive(false);
            addDialog(OnStartDialog);
        }
    }

    public static bool HasDialog()
    {
        return dialogBox.gameObject.activeSelf || objects.Count > 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!dialogBox.gameObject.activeSelf)
        {
            updateDialog();
        }
    }

    void updateDialog()
    {
        while (objects.Count > 0 && !dialogBox.gameObject.activeSelf)
        {
            ManagerObject newSpawningObject = objects.Dequeue();
            if (newSpawningObject.GetType().Name.Equals("Dialog"))
            {
                dialogBox.gameObject.SetActive(true);
                currentDialog = (Dialog)newSpawningObject;
                dialogText.text = currentDialog.text;
                dialogText.color = currentDialog.color;
                Time.timeScale = 0f;
            }
            else
            {
                Prefab dequeuedPrefab = (Prefab)newSpawningObject;
                GameObject spawnedPrefab = Instantiate(dequeuedPrefab.prefab);
                spawnedPrefab.transform.position = dequeuedPrefab.position;
            }
        }
    }

    public static void addDialog(string text, bool isColorBlack=true)
    {
        if(isColorBlack)
            objects.Enqueue(new Dialog(text, Color.black));
        else
            objects.Enqueue(new Dialog(text, Color.red));
    }
    public static void addPrefab(GameObject prefab, Vector3 position)
    {
        objects.Enqueue(new Prefab(prefab, position));
    }

    public void closeCurrentDialog()
    {
        dialogBox.gameObject.SetActive(false);
        Time.timeScale = 1f;
        updateDialog();
    }
}
