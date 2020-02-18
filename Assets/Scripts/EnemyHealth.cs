using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth;
    private float currentHealth;
    public EnemyController enemyController;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void takeDamage(float damage) {
        currentHealth -= damage;

        if(currentHealth <= 0f)
            enemyController.handleEnemyDeath();
        else
            enemyController.handleKnockback(0.4f);
    }
}
