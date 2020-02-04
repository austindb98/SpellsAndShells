using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillsController : MonoBehaviour
{

    [System.Serializable]
    public struct SkillUI
    {
        public Image icon;
        public Image outline;
        public Button button;
    }

    public static readonly uint EarthI = 1;
    public static readonly uint IceI = 2;
    public static readonly uint WaterI = 3;
    public static readonly uint ForcefieldI = 4;
    public static readonly uint RingOfFireI = 5;
    public static readonly uint DrinkI = 6;
    public static readonly uint WindI = 7;
    public static readonly uint FirestormI = 8;
    public static readonly uint FireI = 9;
    public static readonly uint WhirlwindI = 10;
    public static readonly uint LightningI = 11;
    public static readonly uint ChaosI = 12;
    public static readonly uint LightI = 13;


    public SkillUI iceUI;
    public SkillUI forcefieldUI;
    public SkillUI ringOfFireUI;
    public SkillUI drinkUI;
    public SkillUI firestormUI;
    public SkillUI whirlwindUI;
    public SkillUI lightningUI;
    public Text skillPoints;




    public Text[] counters = new Text[13];
    public Sprite skillActive;
    public Sprite skillInactive;

    public BasePlayer player;

    private AudioSource menuChange;
    private AudioSource menuError;

    // Start is called before the first frame update
    void Start()
    {
        menuChange = GetComponent<AudioSource>();
        menuError = GetComponents<AudioSource>()[1];

        UnlockSkills();
        AssignSkills();
        SetSkillText();
        SetSkillCounters();
    }

    private void SetSkillCounters()
    {
        for (uint i = 0; i < counters.Length; ++i)
        {
            if (counters[i] != null)
            {
                SetSkillCounter(i);
            }
        }
    }

    private void SetSkillCounter(uint index)
    {
        counters[index].text = "" + player.skillpoints[index + 1];
    }

    private void SetSkillText()
    {
        skillPoints.text = "Skill Points: " + player.skillpoints[0];
        
    }

    private void ActivateSkill(BasePlayer.Skill skill)
    {
        switch (skill)
        {
            case BasePlayer.Skill.Ice:
                iceUI.outline.sprite = skillActive;
                break;
            case BasePlayer.Skill.Forcefield:
                forcefieldUI.outline.sprite = skillActive;
                break;
            case BasePlayer.Skill.RingofFire:
                ringOfFireUI.outline.sprite = skillActive;
                break;
            case BasePlayer.Skill.Drink:
                drinkUI.outline.sprite = skillActive;
                break;
            case BasePlayer.Skill.Firestorm:
                firestormUI.outline.sprite = skillActive;
                break;
            case BasePlayer.Skill.Whirlwind:
                whirlwindUI.outline.sprite = skillActive;
                break;
            case BasePlayer.Skill.Lightning:
                lightningUI.outline.sprite = skillActive;
                break;
            default:
                break; //Don't need to do anything for null skills
        }
    }

    public void OnIncrement(uint skillIndex)
    {
        if (player.skillpoints[0] == 0)
        {
            menuError.Play();
            return;
        }
        menuChange.Play();
        player.skillpoints[0]--;
        player.skillpoints[skillIndex]++;
        SetSkillText();
        SetSkillCounter(skillIndex - 1);
    }

    public void OnDecrement(uint skillIndex)
    {
        if (player.skillpoints[skillIndex] == 0)
        {
            menuError.Play();
            return;
        }
        menuChange.Play();
        player.skillpoints[skillIndex]--;
        player.skillpoints[0]++;
        SetSkillText();
        SetSkillCounter(skillIndex - 1);
    }



    private void UnlockSkills()
    {
        if (player.skillpoints[2] != 0)
        {
            iceUI.outline.enabled = true;
            iceUI.button.enabled = true;
        }
        if (player.skillpoints[4] != 0)
        {
            forcefieldUI.outline.enabled = true;
            forcefieldUI.button.enabled = true;
        }
        if (player.skillpoints[5] != 0)
        {
            ringOfFireUI.outline.enabled = true;
            ringOfFireUI.button.enabled = true;
        }
        if (player.skillpoints[6] != 0)
        {
            drinkUI.outline.enabled = true;
            drinkUI.button.enabled = true;
        }
        if (player.skillpoints[8] != 0)
        {
            firestormUI.outline.enabled = true;
            firestormUI.button.enabled = true;
        }
        if (player.skillpoints[10] != 0)
        {
            whirlwindUI.outline.enabled = true;
            whirlwindUI.button.enabled = true;
        }
        if (player.skillpoints[11] != 0)
        {
            lightningUI.outline.enabled = true;
            lightningUI.button.enabled = true;
        }
    }

    private void AssignSkills()
    {
        if (player.skill1 != BasePlayer.Skill.Null)
        {
            ActivateSkill(player.skill1);
        }
        if (player.skill2 != BasePlayer.Skill.Null)
        {
            ActivateSkill(player.skill2);
        }
        if (player.skill3 != BasePlayer.Skill.Null)
        {
            ActivateSkill(player.skill3);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    /*
     * Below are the button handlers for incrementing and decrementing skill points.
     * No need to look below unless the (+) or (-) button logic is flawed.
     * They all call OnIncrement and OnDecrement methods above
     */



    public void IncrementEarth()
    {
        OnIncrement(EarthI);
    }

    public void DecrementEarth()
    {
        if (player.skillpoints[IceI] != 0 || player.skillpoints[ForcefieldI] != 0 || player.skillpoints[RingOfFireI] != 0)
        {
            menuError.Play();
            return;
        }
        OnDecrement(EarthI);
    }

    public void IncrementIce()
    {
        if (player.skillpoints[EarthI] == 0 || player.skillpoints[WaterI] == 0)
        {
            menuError.Play();
            return;
        }
        OnIncrement(IceI);
        UnlockSkills();
    }

    public void DecrementIce()
    {
        OnDecrement(IceI);
    }

    public void IncrementWater()
    {
        OnIncrement(WaterI);
    }

    public void DecrementWater()
    {
        if (player.skillpoints[IceI] != 0 || player.skillpoints[DrinkI] != 0)
        {
            menuError.Play();
            return;
        }
        OnDecrement(WaterI);
    }

    public void IncrementForcefield()
    {
        if (player.skillpoints[EarthI] == 0 || player.skillpoints[WindI] == 0)
        {
            menuError.Play();
            return;
        }
        OnIncrement(ForcefieldI);
        UnlockSkills();
    }

    public void DecrementForcefield()
    {
        OnDecrement(ForcefieldI);
    }

    public void IncrementRingOfFire()
    {
        if (player.skillpoints[EarthI] == 0 || player.skillpoints[FireI] == 0)
        {
            menuError.Play();
            return;
        }
        OnIncrement(RingOfFireI);
        UnlockSkills();
    }

    public void DecrementRingOfFire()
    {
        
        OnDecrement(RingOfFireI);
    }

    public void IncrementDrink()
    {
        if (player.skillpoints[WaterI] == 0 || player.skillpoints[FireI] == 0)
        {
            menuError.Play();
            return;
        }
        OnIncrement(DrinkI);
        UnlockSkills();
    }

    public void DecrementDrink()
    {
        OnDecrement(DrinkI);
    }

    public void IncrementWind()
    {
        OnIncrement(WindI);
    }

    public void DecrementWind()
    {
        if (player.skillpoints[ForcefieldI] != 0 || player.skillpoints[FirestormI] != 0 || player.skillpoints[WhirlwindI] != 0)
        {
            menuError.Play();
            return;
        }
        OnDecrement(WindI);
    }


    public void IncrementFirestorm()
    {
        if (player.skillpoints[WindI] == 0 || player.skillpoints[FireI] == 0)
        {
            menuError.Play();
            return;
        }
        OnIncrement(FirestormI);
        UnlockSkills();
    }

    public void DecrementFirestorm()
    {
        OnDecrement(FirestormI);
    }

    public void IncrementFire()
    {
        OnIncrement(FireI);
    }

    public void DecrementFire()
    {
        if (player.skillpoints[DrinkI] != 0 || player.skillpoints[FirestormI] != 0 || player.skillpoints[LightningI] != 0)
        {
            menuError.Play();
            return;
        }
        OnDecrement(FireI);
    }

    public void IncrementWhirlwind()
    {
        if (player.skillpoints[WindI] == 0 || player.skillpoints[ChaosI] == 0)
        {
            menuError.Play();
            return;
        }
        OnIncrement(WhirlwindI);
        UnlockSkills();
    }

    public void DecrementWhirlwind()
    {
        
        OnDecrement(WhirlwindI);
    }

    public void IncrementLightning()
    {
        if (player.skillpoints[FireI] == 0 || player.skillpoints[LightI] == 0)
        {
            menuError.Play();
            return;
        }
        OnIncrement(LightningI);
        UnlockSkills();
    }

    public void DecrementLightning()
    {

        OnDecrement(LightningI);
    }

    public void IncrementChaos()
    {
        OnIncrement(ChaosI);
    }

    public void DecrementChaos()
    {
        if (player.skillpoints[WhirlwindI] != 0)
        {
            menuError.Play();
            return;
        }
        OnDecrement(ChaosI);
    }


    public void IncrementLight()
    {
        OnIncrement(LightI);
    }

    public void DecrementLight()
    {
        if (player.skillpoints[LightningI] != 0)
        {
            menuError.Play();
            return;
        }
        OnDecrement(LightI);
    }

   
}
