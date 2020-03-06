using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

class Dialog
{
    

    public string text;
    public float activeTime;
    public float closeTime;
    public Dialog(string text, float activeTime)
    {
        this.text = text;
        this.activeTime = activeTime;
    }
}

public class DialogScheduler : MonoBehaviour
{
    static readonly float DefaultReadTime = 3f;
    private static Transform dialogBox;
    private static Text dialogText;
    private static Queue<Dialog> dialogs;
    private static Dialog currentDialog;

    public string OnStartDialog;
    // Start is called before the first frame update
    void Start()
    {
        dialogs = new Queue<Dialog>();
        dialogBox = transform.Find("DialogBox");
        dialogText = dialogBox.GetComponentInChildren<Text>();
        dialogBox.gameObject.SetActive(false);
        if (OnStartDialog != null && OnStartDialog != "")
        {
            dialogBox.gameObject.SetActive(false);
            addDialog(OnStartDialog, DefaultReadTime);
        }
    }

    public static bool HasDialog()
    {
        return dialogs.Count > 0;
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

    public static void addDialog(string text, float activeTime)
    {
        dialogs.Enqueue(new Dialog(text, activeTime));
    }

    public static void closeCurrentDialog()
    {
        dialogBox.gameObject.SetActive(false);
    }
}
