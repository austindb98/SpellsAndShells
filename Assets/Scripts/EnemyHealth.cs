using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth;
    private float currentHealth;
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
        print(currentHealth);
        if(currentHealth <= 0f)
            Destroy(gameObject);
        else {
            EnemyArcherBoyGraphics eabg = gameObject.GetComponent<EnemyArcherBoyGraphics>();
            if(eabg != null)
                eabg.handleKnockback();
            EnemyTreantGraphics etg = gameObject.GetComponent<EnemyTreantGraphics>();
            if(etg != null)
                etg.handleKnockback();
        }
    }
}
