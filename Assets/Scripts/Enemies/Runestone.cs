using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Runestone : EnemyController
{
    public BaseAttack.Element type;
    public Sprite[] backgrounds = new Sprite[4];
    private SpriteRenderer sr;
    public GameObject prefabDrop;

    public Animator animator;

    private AudioSource hitAudio;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        sr = GetComponent<SpriteRenderer>();
        hitAudio = GetComponent<AudioSource>();
        SetType();
    }

    void SetType()
    {
        sr.sprite = backgrounds[(uint)type];
        base.enemyHealth.weakness = type;
        animator.SetInteger("type", (int) type);
    }

    public override void handleShotgunAttack(int dmg) {
        base.enemyHealth.takeDamage(0, BaseAttack.Element.Normal);
    }

    public override void handleAttack(float damage, BaseAttack.Element type)
    {
        if (type == this.type)
        {
            base.enemyHealth.takeDamage(damage, type);
            animator.SetTrigger("hit");
            if (!hitAudio.isPlaying)
            {
                hitAudio.Play();
            }

        } else
        {
            base.enemyHealth.takeDamage(0, type);
        }
    }

    public override void handleEnemyDeath() {
        Instantiate(prefabDrop, transform.position, Quaternion.identity);
        SoundController.playStoneDestroy();
        Destroy(gameObject);
    }

    public override void applyFireDotEffect(float dotDuration, float dotFrequency, float dotDamage) { }
    public override void applyWindKnockbackEffect(float knockbackMagnitude) { }

    public override void applyFrostSlowingEffect(float magnitude, float time) { }



}
