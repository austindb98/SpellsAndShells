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


    // Start is called before the first frame update
    protected virtual void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public virtual void takeDamage(float damage, BaseAttack.Element type) {
        Color dmgColor = yellowColor;
        string dmg = "";
        if(type == weakness && weakness != BaseAttack.Element.Normal) {
            damage *= weakMult;
            dmgColor = redColor;
            dmg = "!";
        } else if (type == resistance && resistance != BaseAttack.Element.Normal) {
            damage /= weakMult;
            dmgColor = blackColor;
        } else {
        }
        int intDamage = (int) damage;
        dmg = intDamage.ToString() + dmg;
        currentHealth -= intDamage;
        
        Vector3 popupPos = transform.position + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
        

        DamageController.CreatePopup(dmg, popupPos, dmgColor);

        if (!enemyController) // not moveable ignore, non moveable enemy will handle
        {
            return;
        }

        if (currentHealth <= 0f)
        {
            enemyController.handleEnemyDeath();
        }
        else
        {
            enemyController.handleShotgunHit(0.4f);
        }
    }
}
