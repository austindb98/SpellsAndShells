using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogTest : MonoBehaviour
{
    public DialogScheduler dialogScheduler;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("started the red");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("entered the red");
        PlayerController player = collision.GetComponentInParent<PlayerController>();
        if (player)
        {
            if (transform.name.Equals("short"))
            {
                dialogScheduler.addDialog("waiting for 2 seconds", 2);
            }
            if (transform.name.Equals("long"))
            {
                dialogScheduler.closeCurrentDialog();
                dialogScheduler.addDialog("waiting for 5 seconds", 5);
            }
            if (transform.name.Equals("indefinite"))
            {
                dialogScheduler.closeCurrentDialog();
                dialogScheduler.addDialog("waiting forever", 0);
            }
            Destroy(gameObject);
        }
    }
}
