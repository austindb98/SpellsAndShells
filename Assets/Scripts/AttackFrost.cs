using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackFrost : BaseAttack
{
    private float frostDmg = 50f;
    // Start is called before the first frame update
    void Start()
    {
        speed = 8;
        damage = 2;
    }

    
}
