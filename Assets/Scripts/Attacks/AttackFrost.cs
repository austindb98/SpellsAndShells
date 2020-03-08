using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackFrost : BaseAttack
{
    private float frostDmg = 50f;

    private float slowMag = 0.6f;
    private float slowDur = 10f;

    public IceType iceType;

    public enum IceType
    {
        Frost, Freeze, Blizzard, IceAge
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        element = Element.Ice;
    }

    protected override void OnTriggerEnter2D(Collider2D collider) {
        EnemyController enemyController = collider.gameObject.GetComponent<EnemyController>();
        if(enemyController && enemyController.aiPath != null)
            enemyController.applyFrostSlowingEffect(slowMag, slowDur);
        base.OnTriggerEnter2D(collider);
    }


}
