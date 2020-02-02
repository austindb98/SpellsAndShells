using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudController : MonoBehaviour
{
    static readonly bool Debug = true;
    static readonly float MaxMana = 180;
    static readonly float MaxHealth = 180;

    private static readonly float DebugFillSpeed = 30;

    public RectTransform health;
    public RectTransform mana;
    public Image bulletIcon;
    public RectTransform bulletOverlay;
    public Image spell1;
    public Image spell2;
    public Image spell3;
    public Image spell1Icon;
    public Image spell2Icon;
    public Image spell3Icon;

    private float healthAmt;
    private float manaAmt;
    
    void Start()
    {
        SetHealth(MaxHealth);
    }

    public void SetMana(float uiAmt)
    {
        manaAmt = uiAmt;
        if (manaAmt > MaxMana)
        {
            manaAmt = MaxMana;
        }
        else if (manaAmt < 0)
        {
            manaAmt = 0;
        }
        mana.localScale = new Vector3(manaAmt / MaxMana, 1, 1);
    }

    public void SetHealth(float uiAmt)
    {
        healthAmt = uiAmt;
        if (healthAmt > MaxHealth)
        {
            healthAmt = MaxHealth;
        }
        else if (healthAmt < 0)
        {
            healthAmt = 0;
        }
        health.localScale = new Vector3(healthAmt / MaxHealth, 1, 1);
    }

    private void DebugUpdate()
    {
        
        if (healthAmt >= MaxHealth)
        {
            SetHealth(0);
        } else
        {
            SetHealth(healthAmt + Time.deltaTime * DebugFillSpeed);
        }

        if (manaAmt >= MaxMana)
        {
            SetMana(0);
        }
        else
        {
            SetMana(manaAmt + Time.deltaTime * DebugFillSpeed);
        }
    }
    
    void Update()
    {
        if (Debug)
        {
            DebugUpdate();
            return;
        }
    }
}
