using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PhantomAttack : MonoBehaviour
{

    private CircleCollider2D col;
    private BaseAttack attack;
    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<CircleCollider2D>();
        attack = GetComponent<BaseAttack>();
        //col.enabled = false;
    }

    public void Enable()
    {
        attack.DoneMoving();
    }
}
