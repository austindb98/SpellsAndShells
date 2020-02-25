using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Runestone : EnemyHealth
{

    public BaseAttack.Element type;
    public Sprite[] backgrounds = new Sprite[4];
    private SpriteRenderer sr;
    public GameObject prefabDrop;

    public Animator animator;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        sr = GetComponent<SpriteRenderer>();
        SetType();
    }

    void SetType()
    {
        sr.sprite = backgrounds[(uint)type];
        weakness = type;
        animator.SetInteger("type", (int) type);
    }

    public override void takeDamage(float damage, BaseAttack.Element type)
    {
        if (type == this.type)
        {
            base.takeDamage(damage, type); // type doesn't matter, normal damage
        } else
        {
            base.takeDamage(0, type);
        }

        if (currentHealth <= 0f)
        {
            Instantiate(prefabDrop, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        animator.SetTrigger("hit");
    }


}
