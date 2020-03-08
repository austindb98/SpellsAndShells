using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class AttackFireRing : MonoBehaviour
{
    public float Lifetime = 3f;

    public GameObject parent;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(parent, Lifetime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyController enemyController = collision.gameObject.GetComponent<EnemyController>();
        if (enemyController) {
            enemyController.applyFireDotEffect(AttackFireball.dotDuration, AttackFireball.dotFrequency, AttackFireball.dotDamage);
        }
    }
}
