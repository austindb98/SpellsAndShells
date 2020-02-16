using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudController : MonoBehaviour
{
    static readonly bool Debug = false;
    static readonly float MaxMana = 180;
    static readonly float MaxHealth = 180;
    

    private static readonly float DebugFillSpeed = 30;
    private static readonly float DebugSkillTime = 3;
    private static readonly float DebugShootTime = 1.5f;

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
    public Sprite skillActivated;
    public Sprite skillInactive;
    public float shootCooldownTime = .75f;
    public BasePlayer player;
    public Sprite[] skillIcons = new Sprite[7];

    private float healthAmt;
    private float manaAmt;
    private Slot activated;
    private float shootCooldownTimer;
    private float debugSkillTimer;
    private float debugShootTimer;
    private int skillIndex = 0;
    

    public enum Slot
    {
        Slot1, Slot2, Slot3
    }
    
    void Start()
    {
        SetHealth(player.MaxHealth);
        SetMana(player.MaxMana);
        SetSlot(Slot.Slot1, skillActivated);
        skillIndex = player.spellIndex;
    }

    public void SetMana(float uiAmt)
    {
        manaAmt = uiAmt;
        if (manaAmt > player.MaxMana)
        {
            manaAmt = player.MaxMana;
        }
        else if (manaAmt < 0)
        {
            manaAmt = 0;
        }
        mana.localScale = new Vector3(manaAmt / player.MaxMana, 1, 1);
    }

    public void SetHealth(float uiAmt)
    {
        healthAmt = uiAmt;
        if (healthAmt > player.MaxHealth)
        {
            healthAmt = player.MaxHealth;
        }
        else if (healthAmt < 0)
        {
            healthAmt = 0;
        }
        health.localScale = new Vector3(healthAmt / player.MaxHealth, 1, 1);
    }

    public void PreviousSlot()
    {
        switch (activated)
        {
            case Slot.Slot1:
                ActivateSlot(Slot.Slot3);
                break;
            case Slot.Slot2:
                ActivateSlot(Slot.Slot1);
                break;
            case Slot.Slot3:
                ActivateSlot(Slot.Slot2);
                break;
        }
    }

    public void SetSlot(int index)
    {
        switch (index)
        {
            case 2:
                ActivateSlot(Slot.Slot3);
                break;
            case 1:
                ActivateSlot(Slot.Slot2);
                break;
            case 0:
                ActivateSlot(Slot.Slot1);
                break;
        }
    }

    public void NextSlot()
    {
        switch (activated)
        {
            case Slot.Slot1:
                ActivateSlot(Slot.Slot2);
                break;
            case Slot.Slot2:
                ActivateSlot(Slot.Slot3);
                break;
            case Slot.Slot3:
                ActivateSlot(Slot.Slot1);
                break;
        }
    }

    public void ActivateSlot(Slot which)
    {
        if (which == activated)
        {
            return;
        }
        SetSlot(which, skillActivated);
        SetSlot(activated, skillInactive);
        activated = which;
    }

    public void SetShot()
    {
        shootCooldownTimer = 0;
    }
    
    private void SetSlot(Slot which, Sprite sprite)
    {
        switch (which)
        {
            case Slot.Slot1:
                spell1.sprite = sprite;
                break;
            case Slot.Slot2:
                spell2.sprite = sprite;
                break;
            case Slot.Slot3:
                spell3.sprite = sprite;
                break;
        }
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

        debugSkillTimer += Time.deltaTime;
        if (debugSkillTimer >= DebugSkillTime)
        {
            NextSlot();
            debugSkillTimer = 0;
        }
        debugShootTimer += Time.deltaTime;
        if (debugShootTimer >= DebugShootTime)
        {
            SetShot();
            debugShootTimer = 0;
        }
    }


    
    void Update()
    {
        if (Debug)
        {
            DebugUpdate();
        }

       

        if (!player.shotReady)
        {
            if (shootCooldownTimer < shootCooldownTime)
            {
                shootCooldownTimer += Time.deltaTime;
                bulletOverlay.localScale = new Vector3(1, 1 - (shootCooldownTimer / shootCooldownTime), 1);
            }
            else
            {
                shootCooldownTimer = 0;
                player.shotReady = true;
            }
        }

        

        if (player.health != healthAmt)
        {
            SetHealth(player.health);
        }

        if (player.mana != manaAmt)
        {
            SetMana(player.mana);
        }
        
        if (skillIndex != player.spellIndex)
        {
            skillIndex = player.spellIndex;
            SetSlot(skillIndex);
        }

        
    }
}
