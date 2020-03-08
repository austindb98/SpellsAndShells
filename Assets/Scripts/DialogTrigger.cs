using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogTrigger : MonoBehaviour
{
    public string[] dialogs;
    public GameObject[] prefabs;
    public Vector3[] positions;
    // use "prefab" and "dialog" to indicate the ordering
    // of how given objects should be added to the queue
    public string[] ordering;
    
    void Start()
    {
        //simple check to determine if:
        //  -combined number prefabs and dialogs are equal to the number of ordering strings
        //  -number of positions is equal to the number of prefabs
        //DOES NOT GUARANTEE CORRECTNESS OF SETUP.  JUST INITIAL CHECK.
        if((dialogs.Length + prefabs.Length != ordering.Length) || prefabs.Length != positions.Length)
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        int prefabIndex = 0;
        int dialogIndex = 0;
        for (int i = 0; i < ordering.Length; i++)
        {
            if (ordering[i].Equals("dialog")) //add a dialog
            {
                DialogScheduler.addDialog(dialogs[dialogIndex]);
                dialogIndex++;
            }
            else //add a prefab
            {
                DialogScheduler.addPrefab(prefabs[prefabIndex], positions[prefabIndex]);
                prefabIndex++;
            }
        }
        Destroy(gameObject);
    }

}
