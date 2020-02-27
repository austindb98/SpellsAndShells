using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

class Dialog
{
    public string text;
    public int activeTime;
    public float closeTime;
    public Dialog(string text, int activeTime)
    {
        this.text = text;
        this.activeTime = activeTime;
    }
}

public class DialogScheduler : MonoBehaviour
{
    private Transform dialogBox;
    private Text dialogText;
    private Queue<Dialog> dialogs;
    private Dialog currentDialog;
    // Start is called before the first frame update
    void Start()
    {
        dialogs = new Queue<Dialog>();
        dialogBox = transform.Find("DialogBox");
        dialogText = dialogBox.GetComponentInChildren<Text>();
        dialogBox.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (dialogBox.gameObject.activeSelf)
        {
            if (Time.time > currentDialog.closeTime && currentDialog.activeTime > 0)
            {
                dialogBox.gameObject.SetActive(false);
                updateDialog();
            }
        }
        else
        {
            updateDialog();
        }
    }

    void updateDialog()
    {
        if (dialogs.Count > 0)
        {
            dialogBox.gameObject.SetActive(true);
            currentDialog = dialogs.Dequeue();
            currentDialog.closeTime = Time.time + currentDialog.activeTime;
            dialogText.text = currentDialog.text;
        }
    }

    public void addDialog(string text, int activeTime)
    {
        dialogs.Enqueue(new Dialog(text, activeTime));
    }

    public void closeCurrentDialog()
    {
        dialogBox.gameObject.SetActive(false);
    }
}
