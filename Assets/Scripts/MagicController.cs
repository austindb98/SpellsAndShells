using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (BasePlayer))]
public class MagicController : MonoBehaviour
{

    public Spell[] spells = new Spell[12];

    static readonly float MouseScrollSensitivity = .001f;
    static readonly float NumberOfScrolls = 2;

    private int mouseScrolls;

    private BasePlayer player;
    private AudioSource castSound;

    private Spell currentSpell;

    private Quaternion spawnRotation;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<BasePlayer>();
        castSound = GetComponent<AudioSource>();
    }

    public Spell CalculateCurrentSpell()
    {
        switch (player.spellIndex)
        {
            case 0:
                if (player.skillpoints[1] == SkillsController.LevelCutoff4)
                {
                    return spells[3];
                }
                else if (player.skillpoints[1] >= SkillsController.LevelCutoff3)
                {
                    return spells[2];
                }
                else if (player.skillpoints[1] >= SkillsController.LevelCutoff2)
                {
                    return spells[1];
                }
                else if (player.skillpoints[1] >= SkillsController.LevelCutoff1)
                {
                    return spells[0];
                }
                break;
            case 1:
                if (player.skillpoints[2] == SkillsController.LevelCutoff4)
                {
                    return spells[7];
                }
                else if (player.skillpoints[2] >= SkillsController.LevelCutoff3)
                {
                    return spells[6];
                }
                else if (player.skillpoints[2] >= SkillsController.LevelCutoff2)
                {
                    return spells[5];
                }
                else if (player.skillpoints[2] >= SkillsController.LevelCutoff1)
                {
                    return spells[4];
                }
                break;
            case 2:
                if (player.skillpoints[3] == SkillsController.LevelCutoff4)
                {
                    return spells[11];
                }
                else if (player.skillpoints[3] >= SkillsController.LevelCutoff3)
                {
                    return spells[10];
                }
                else if (player.skillpoints[3] >= SkillsController.LevelCutoff2)
                {
                    return spells[9];
                }
                else if (player.skillpoints[3] >= SkillsController.LevelCutoff1)
                {
                    return spells[8];
                }
                break;
        }

        return null;
    }
    
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (currentSpell == null || currentSpell.attackPrefab == null || currentSpell.manaCoolDown > player.mana)
            {
                return;
            }
            player.UseMana(currentSpell.manaCoolDown);
            castSound.Play();
            Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            float angle = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg + 90;
            //Vector2.Angle(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position);
            Debug.Log("cxast angle: " + angle);
            spawnRotation = Quaternion.AngleAxis(angle, Vector3.forward);
            
            Instantiate(currentSpell.attackPrefab, transform.position, spawnRotation, transform.parent);
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            mouseScrolls++;
            if (mouseScrolls >= NumberOfScrolls)
            {
                player.NextSpell();
                mouseScrolls = 0;
            }
            currentSpell = CalculateCurrentSpell();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            
            mouseScrolls--;
            if (mouseScrolls <= -NumberOfScrolls)
            {
                player.PreviousSpell();
                mouseScrolls = 0;
            }
            currentSpell = CalculateCurrentSpell();
        }
    }
}
