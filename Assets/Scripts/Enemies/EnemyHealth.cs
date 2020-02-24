using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public BaseAttack.Element weakness;
    public BaseAttack.Element resistance;
    private float weakMult = 2;
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

    public void takeDamage(float damage, BaseAttack.Element type) {
        Color dmgColor = Color.yellow;
        string dmg;
        int intDamage = (int) damage * 5; //Buff for testing
        if(type == weakness) {
            intDamage *= (int) weakMult;
            dmgColor = Color.red;
            dmg = intDamage.ToString() + "!";
        } else if (type == resistance) {
            intDamage /= (int) weakMult;
            dmgColor = Color.black;
            dmg = intDamage.ToString();
        } else {
            dmg = intDamage.ToString();
        }
        currentHealth -= intDamage;
        
        Vector3 popupPos = transform.position + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);

        string testDam = "" + Random.Range(-2, 30);

        DamageController.CreatePopup(testDam, popupPos, dmgColor);

        if(currentHealth <= 0f)
            enemyController.handleEnemyDeath();
        else
            enemyController.handleShotgunHit(0.4f);
    }
}
