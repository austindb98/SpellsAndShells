using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    public GameObject skillUi;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Skills"))
        {
            skillUi.SetActive(!skillUi.activeInHierarchy);
        }
    }
}
