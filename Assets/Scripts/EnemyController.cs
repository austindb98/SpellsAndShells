using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyController : MonoBehaviour
{

    public float attackStrength;
    // Start is called before the first frame update
    abstract public void Start();

    // Update is called once per frame
    abstract public void Update();

    abstract public void handleShotgunHit(float knockbackMagnitude);

    abstract public void handleEnemyDeath();
}
