using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth;
    private float currentHealth;
    public EnemyController enemyController;
    public ParticleSystem damageText;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        damageText = Resources.Load<ParticleSystem>("ParticleSystems/TextParticles");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void takeDamage(float damage) {
        currentHealth -= damage;
        ParticleSystem text =  Instantiate(damageText);
        text.transform.position = transform.position + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
        text.GetComponentInChildren<UnityEngine.UI.Text>().text = damage.ToString();

        
        text.Play();
        if(currentHealth <= 0f)
            enemyController.handleEnemyDeath();
        else
            enemyController.handleKnockback(0.4f);
    }
}
