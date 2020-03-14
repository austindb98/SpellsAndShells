using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public abstract class BaseAttack : MonoBehaviour
{

    public enum Element
    {
        Normal, Wind, Ice, Fire
    };

    public float speed;
    public float damage;
    protected Element element;

    protected ParticleSystem particles;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        particles = GetComponent<ParticleSystem>();

    }

    // Update is called once per frame
    protected virtual void Update()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);
    }

    protected virtual void OnDeath()
    {
        Destroy(gameObject);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "DestructibleSpell")
            return;
        if (collision.gameObject.layer == LayerMask.NameToLayer("Entities") || collision.gameObject.layer == LayerMask.NameToLayer("StationaryEntities"))
        {
            EnemyController enemyController = collision.gameObject.GetComponent<EnemyController>();
            enemyController.handleAttack(damage, element);
        }
        OnDeath();
    }

}
