using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackFireball : BaseAttack
{
    private float dotDuration = 5f;
    private float dotFrequency = .5f;    // how often to inflict damage
    private float dotDamage = 2f;

    Vector2 explosionLocation = Vector2.negativeInfinity;
    private float explosionRadius = 10f;

    public FireType fireType;

    public enum FireType
    {
        Fireball, Meteorite, MeteorShower, Armageddon
    }

    // Start is called before the first frame update
    void Start()
    {
        element = Element.Fire;
    }

    // Update is called once per frame
    protected override void Update()
    {
        if(explosionLocation != Vector2.negativeInfinity) {
            TriggerExplosion(explosionLocation);
            explosionLocation = Vector2.negativeInfinity;
        }
        base.Update();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyController enemyController = collision.gameObject.GetComponent<EnemyController>();
        if(enemyController)
            enemyController.applyFireDotEffect(dotDuration, dotFrequency, dotDamage);
        if (fireType >= FireType.Meteorite)
        {
            if(explosionLocation != Vector2.negativeInfinity) {
                explosionLocation = collision.gameObject.transform.position;
                Debug.Log("HIT enemy at " + explosionLocation);
            }
        }
        base.OnTriggerEnter2D(collision);
    }

    void TriggerExplosion(Vector2 center) {
        Collider[] hitColliders = Physics.OverlapSphere(center, explosionRadius);
        for(int i = 0; i < hitColliders.Length; i++) {
            EnemyController enemyController = hitColliders[i].gameObject.GetComponent<EnemyController>();
            if(enemyController != null) {
                Debug.Log("DOT enemy at " + hitColliders[i].gameObject.transform.position);
                enemyController.applyFireDotEffect(dotDuration, dotFrequency, dotDamage);
            }
        }
    }
}
