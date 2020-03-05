using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAttack : MonoBehaviour
{

    public enum Element
    {
        Normal, Wind, Ice, Fire
    };

    public float speed;
    public float damage;
    protected Element element;
    // Start is called before the first frame update
    protected abstract void Start();

    // Update is called once per frame
    protected virtual void Update()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);
    }



    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Entities") || collision.gameObject.layer == LayerMask.NameToLayer("StationaryEntities"))
        {
            EnemyController enemyController = collision.gameObject.GetComponent<EnemyController>();
            enemyController.handleAttack(damage, element);
        }
        Destroy(gameObject);
    }

}
