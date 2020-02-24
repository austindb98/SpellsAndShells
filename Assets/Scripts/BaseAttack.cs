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
    protected float damage;
    protected int element;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    protected virtual void Update()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);
    }



    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Entities"))
        {
            EnemyHealth enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();
            enemyHealth.takeDamage(damage);
        }
        Destroy(gameObject);
    }

}
