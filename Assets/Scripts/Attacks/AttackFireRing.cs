using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class AttackFireRing : MonoBehaviour
{
    public float Lifetime = 3f;

    public AttackFireball parent;

    private CircleCollider2D ring;

    bool attackHappened;

    float scaler = 0;

    static readonly float scaleSpeed = .75f;
    static readonly float lingerTime = .55f;

    // Start is called before the first frame update
    void Start()
    {
        ring = GetComponent<CircleCollider2D>();
        transform.rotation = Quaternion.Euler(0, 0, 0);
        transform.localScale = Vector3.one * scaler;
    }

    // Update is called once per frame
    void Update()
    {
        if (scaler < 1)
        {
            scaler += scaleSpeed * Time.deltaTime;
            transform.localScale = Vector3.one * scaler;
        }
        else if (scaler > 1)
        {
            Destroy(parent.gameObject, lingerTime);
        }
    }

    /*private void LateUpdate()
    {
        if (attackHappened == true)
        {
            ring.enabled = false;
        }
    }*/

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyController enemyController = collision.gameObject.GetComponent<EnemyController>();
        if (enemyController && enemyController.gameObject.tag != "DestructibleSpell") {
            enemyController.applyFireDotEffect(AttackFireball.dotDuration, AttackFireball.dotFrequency, AttackFireball.dotDamage);
            enemyController.HandleMercyAttack(parent.damage, BaseAttack.Element.Fire);
            attackHappened = true;
        }
    }
}
