using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * Just a class to abstract some of the ring attack code
 */
[RequireComponent(typeof(CircleCollider2D))]
public abstract class ChildAttack : MonoBehaviour
{

    public float Lifetime = 3f;
    public GameObject parent;

    protected virtual void Start()
    {
        Destroy(parent, Lifetime);
    }
}
