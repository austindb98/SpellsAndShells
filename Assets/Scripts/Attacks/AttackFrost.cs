using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackFrost : BaseAttack
{
    public static float frostDmg = 50f;

    public static float slowMag = 0.6f;
    public static float slowDur = 10f;

    public IceType iceType;

    public GameObject ringOfIce;

    public enum IceType
    {
        Frost, Freeze, Blizzard, IceAge
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        element = Element.Ice;
    }

    protected override void OnDeath()
    {

        if (iceType >= IceType.Freeze)
        {
            Destroy(this);
        }
        else
        {
            base.OnDeath();
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collider) {
        EnemyController enemyController = collider.gameObject.GetComponent<EnemyController>();
        if(enemyController && enemyController.aiPath != null)
            enemyController.applyFrostSlowingEffect(slowMag, slowDur);
        if (iceType >= IceType.Freeze)
        {
            ringOfIce.SetActive(true);

        }
        base.OnTriggerEnter2D(collider);
    }


}
