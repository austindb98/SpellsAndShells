using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAttack : MonoBehaviour
{

    public enum Element
    {
        Wind, Ice, Fire
    };

    protected float speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);
    }

    protected void DefaultOnCollision()
    {
        Destroy(gameObject);
    }
    
}
