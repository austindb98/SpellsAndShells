using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackFrost : BaseAttack
{
    // Start is called before the first frame update
    void Start()
    {
        speed = 1.5f;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        base.DefaultOnCollision();
    }
}
