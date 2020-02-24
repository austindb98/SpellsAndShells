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
    private float damageUpdateTime;
    private static float deltaDamageTime;
    private float accumulatedDamage;



    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        damageText = Resources.Load<ParticleSystem>("ParticleSystems/TextParticles");
        accumulatedDamage = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void takeDamage(float damage, BaseAttack.Element type) {
        Color dmgColor = Color.yellow;
        string dmg;
        dmg = damage.ToString();
        if(type == weakness) {
            damage *= weakMult;
            dmgColor = Color.red;
            dmg = damage.ToString() + "!";
        } else if (type == resistance) {
            damage /= weakMult;
            dmgColor = Color.black;
            dmg = damage.ToString();
        }
        currentHealth -= damage;
        //accumulatedDamage += damage;

        ParticleSystem text = Instantiate(damageText);
        text.transform.position = transform.position + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
        text.GetComponentInChildren<UnityEngine.UI.Text>().text = dmg;
        text.GetComponentInChildren<UnityEngine.UI.Text>().color = dmgColor;
        text.Play();

        if(currentHealth <= 0f)
            enemyController.handleEnemyDeath();
        else
            enemyController.handleShotgunHit(0.4f);
    }
}
