using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsMover : MonoBehaviour
{
    public float moveSpeed = 10;
    public float yMax = 1500;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position +  (Vector3)(Time.deltaTime * moveSpeed * Vector2.up);
        if(transform.position.y > yMax) {
             SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        }
    }
}
