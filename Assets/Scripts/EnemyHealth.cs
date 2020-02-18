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
    private float damageUpdateTime;
    private static float deltaDamageTime;
    private float accumulatedDamage;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        damageText = Resources.Load<ParticleSystem>("ParticleSystems/TextParticles");
        damageUpdateTime = Time.time + deltaDamageTime;
        accumulatedDamage = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(damageUpdateTime < Time.time)
        {
            damageUpdateTime = Time.time + deltaDamageTime;
            if(accumulatedDamage > 0)
            {
                ParticleSystem text = Instantiate(damageText);
                text.transform.position = transform.position + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
                text.GetComponentInChildren<UnityEngine.UI.Text>().text = accumulatedDamage.ToString();
                text.Play();
            }
            accumulatedDamage = 0;
        }
        
    }

    public void takeDamage(float damage) {
        currentHealth -= damage;
        accumulatedDamage += damage;
        if(currentHealth <= 0f)
            enemyController.handleEnemyDeath();
        else
            enemyController.handleShotgunHit(0.4f);
    }
}
