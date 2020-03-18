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

    protected bool canMove;

    private bool attackDone;

    private CircleCollider2D bounds;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        bounds = GetComponent<CircleCollider2D>();
        particles = GetComponent<ParticleSystem>();
        canMove = true;

    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (canMove)
        {
            transform.Translate(Vector3.down * speed * Time.deltaTime);
        }
        
    }

    public virtual void DoneMoving()
    {
        canMove = false;
        //gameObject.SetActive(true);
    }

    public virtual void OnDeath()
    {
        Debug.Log("destroying spell object");
        Destroy(gameObject);
    }


    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if(attackDone || collision.gameObject.tag == "DestructibleSpell" || collision.gameObject.layer == LayerMask.NameToLayer("Unwalkable"))
            return;
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Entities") ||
                 collision.gameObject.layer == LayerMask.NameToLayer("StationaryEntities")) {
            
            EnemyController enemyController = collision.gameObject.GetComponent<EnemyController>();
            enemyController.handleAttack(damage, element);
            

        }
        Debug.Log("collided with layer:" + collision.gameObject.layer);
        attackDone = true;
        OnDeath();
    }

}
