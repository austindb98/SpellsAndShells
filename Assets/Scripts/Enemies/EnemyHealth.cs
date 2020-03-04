using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public BaseAttack.Element weakness;
    public BaseAttack.Element resistance;
    private static readonly float weakMult = 1.5f;
    public float maxHealth;
    protected float currentHealth;
    public EnemyController enemyController;

    static readonly Color redColor = new Color(.6353f, .1373f, .2000f);
    static readonly Color blackColor = new Color(.0941f, .0784f, .1451f);
    static readonly Color yellowColor = new Color(.9961f, .9059f, .3804f);
    private Color dmgColor = yellowColor;
    private float accumulatedDamage = -1; // so that 0 damage still gets a popup


    // Start is called before the first frame update
    protected virtual void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void LateUpdate() {

        if(accumulatedDamage >= 0) {
            int intDamage = (int) accumulatedDamage;
            string dmg = intDamage.ToString();
            if(dmgColor == redColor) {
                dmg += "!";
            }
            Vector3 popupPos = transform.position + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
            DamageController.CreatePopup(dmg, popupPos, dmgColor);
            currentHealth -= intDamage;
        }

        accumulatedDamage = -1;
    }

    public virtual void takeDamage(float damage, BaseAttack.Element type) {
        dmgColor = yellowColor;
        if(type == weakness) {
            damage *= weakMult;
            dmgColor = redColor;
        } else if (type == resistance) {
            damage /= weakMult;
            dmgColor = blackColor;
        }
        if (accumulatedDamage < 0){
            accumulatedDamage = 0;
        }
        accumulatedDamage += damage;

        if (!enemyController) // not moveable ignore, non moveable enemy will handle
            return;

        if (currentHealth <= 0f) {
            enemyController.handleEnemyDeath();
        }
    }

    public void setCurrentHealth(float healthVal) {
        currentHealth = healthVal;
    }
}
