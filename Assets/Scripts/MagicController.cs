using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (BasePlayer))]
public class MagicController : MonoBehaviour
{

    public Spell[] spells = new Spell[12];

    static readonly float MouseScrollSensitivity = .001f;
    static readonly float NumberOfScrolls = 1;

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

    //TODO implement
    private Spell CalculateSpell(uint elementPoints, uint lowestIndex)
    {
        if (elementPoints == SkillsController.LevelCutoff4)
        {
            return spells[lowestIndex + 3];
        }
        else if (elementPoints >= SkillsController.LevelCutoff3)
        {
            return spells[lowestIndex + 2];
        }
        else if (elementPoints >= SkillsController.LevelCutoff2)
        {
            return spells[lowestIndex + 1];
        }
        else if (elementPoints >= SkillsController.LevelCutoff1)
        {
            return spells[lowestIndex];
        }
        return null;
    }

    private Spell CalculateSpell1()
    {
        return CalculateSpell(player.skillpoints[1], 0);
    }

    private Spell CalculateSpell2()
    {
        return CalculateSpell(player.skillpoints[2], 4);
    }

    private Spell CalculateSpell3()
    {
        return CalculateSpell(player.skillpoints[3], 8);
    }

    public Spell CalculateCurrentSpell()
    {
        switch (player.spellIndex)
        {
            case 0:
                return CalculateSpell1();
            case 1:
                return CalculateSpell2();
            case 2:
                return CalculateSpell3();
        }

        return null;
    }

    private void CastSpell()
    {
        if (currentSpell == null || currentSpell.attackPrefab == null || currentSpell.manaCoolDown > player.mana)
        {
            SoundController.PlayError();
        }
        else
        {
            Vector2 magicPosition = (Vector2)transform.position - new Vector2(0,.5f);
            player.UseMana(currentSpell.manaCoolDown);
            castSound.Play();
            Vector2 camPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 difference = camPos - magicPosition;
            float angle = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg + 90;
            spawnRotation = Quaternion.AngleAxis(angle, Vector3.forward);
            if (!currentSpell.targeted)
            {
                Instantiate(currentSpell.attackPrefab, magicPosition, spawnRotation, transform.parent);
            } else
            {
                Instantiate(currentSpell.attackPrefab, camPos, currentSpell.attackPrefab.transform.rotation, transform.parent);
            }
            
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            currentSpell = CalculateCurrentSpell();
            CastSpell();

        }
        else if (Input.GetButtonDown("Spell1"))
        {
            currentSpell = CalculateSpell1();
            CastSpell();
        }
        else if (Input.GetButtonDown("Spell2"))
        {
            currentSpell = CalculateSpell2();
            CastSpell();
        }
        else if (Input.GetButtonDown("Spell3"))
        {
            currentSpell = CalculateSpell3();
            CastSpell();
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            mouseScrolls++;
            if (mouseScrolls >= NumberOfScrolls)
            {
                player.NextSpell();
                mouseScrolls = 0;
            }
            //currentSpell = CalculateCurrentSpell();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {

            mouseScrolls--;
            if (mouseScrolls <= -NumberOfScrolls)
            {
                player.PreviousSpell();
                mouseScrolls = 0;
            }
            //currentSpell = CalculateCurrentSpell();
        }
    }
}
