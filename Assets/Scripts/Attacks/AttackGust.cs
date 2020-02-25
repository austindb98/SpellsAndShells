using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackGust : BaseAttack
{
    // Start is called before the first frame update
    void Start()
    {
        speed = 14;
        damage = 4;
        element = Element.Wind;
    }
}
