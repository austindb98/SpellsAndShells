using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillsController : MonoBehaviour
{



    static readonly float MaxPoints = 15f;
    public static readonly uint LevelCutoff1 = 1;
    public static readonly uint LevelCutoff2 = 2;
    public static readonly uint LevelCutoff3 = 4;
    public static readonly uint LevelCutoff4 = 7;


    public BasePlayer player;

    private AudioSource menuChange;
    private AudioSource menuError;

    public RectTransform unassigned;
    public GameObject[] windFluids = new GameObject[4];
    public GameObject[] iceFluids = new GameObject[4];
    public GameObject[] fireFluids = new GameObject[4];
    public Text windCounter;
    public Text iceCounter;
    public Text fireCounter;
    public Image[] windIcons = new Image[4];
    public Image[] iceIcons = new Image[4];
    public Image[] fireIcons = new Image[4];
    public Sprite[] windUnlocked = new Sprite[4];
    public Sprite[] iceUnlocked = new Sprite[4];
    public Sprite[] fireUnlocked = new Sprite[4];
    public Text unassignedCounter;

    private uint UnassignedPoints {
        get{ return player.skillpoints[0]; }
        set{ player.skillpoints[0] = value; }
    }
    private uint WindPoints
    {
        get { return player.skillpoints[1]; }
        set { player.skillpoints[1] = value; }
    }
    private uint IcePoints
    {
        get { return player.skillpoints[2]; }
        set { player.skillpoints[2] = value; }
    }
    private uint FirePoints
    {
        get { return player.skillpoints[3]; }
        set { player.skillpoints[3] = value; }
    }


    // Start is called before the first frame update
    void Start()
    {
        menuChange = GetComponent<AudioSource>();
        menuError = GetComponents<AudioSource>()[1];
        SetBars();
    }





    private void SetBars()
    {
        SetUnassignedBar();
        //SetElementBar(windFluids, windIcons, windUnlocked, WindPoints, windCounter);
        //SetElementBar(iceFluids, iceIcons, iceUnlocked, IcePoints, iceCounter);
        //SetElementBar(fireFluids, fireIcons, fireUnlocked, FirePoints, fireCounter);
        SetWindBar(WindPoints);
        SetIceBar(IcePoints);
        SetFireBar(FirePoints);
    }

    private void SetWindBar(uint windPoints)
    {
        SetElementBar(windFluids, windIcons, windUnlocked, windPoints, windCounter);
    }

    private void SetIceBar(uint icePoints)
    {
        SetElementBar(iceFluids, iceIcons, iceUnlocked, icePoints, iceCounter);
    }

    private void SetFireBar(uint firePoints)
    {
        SetElementBar(fireFluids, fireIcons, fireUnlocked, firePoints, fireCounter);
    }

    private void SetElementBar(GameObject[] fluids, Image[] icons, Sprite[] enabled, uint elementPoints, Text counter)
    {
        fluids[0].SetActive(false);
        fluids[1].SetActive(false);
        fluids[2].SetActive(false);
        //fluids[3].SetActive(false);
        if (elementPoints >= LevelCutoff1)
        {
            fluids[0].SetActive(true);
            icons[0].sprite = enabled[0];
            if (elementPoints >= LevelCutoff2)
            {
                fluids[1].SetActive(true);
                icons[1].sprite = enabled[1];
                if (elementPoints >= LevelCutoff3)
                {
                    fluids[2].SetActive(true);
                    icons[2].sprite = enabled[2];
                    /*if (elementPoints >= LevelCutoff4)
                    {
                        fluids[3].SetActive(true);
                        icons[3].sprite = enabled[3];
                    }*/
                }
            }
        }
        counter.text = "" + elementPoints;
        player.skillsUpdated = true;
    }

    private void SetUnassignedBar()
    {
        unassigned.localScale = new Vector3(UnassignedPoints / MaxPoints, 1, 1);
        unassignedCounter.text = "" + UnassignedPoints;
    }

    private bool PlayErrorFalse(bool predicate)
    {
        if (!predicate) {
            menuError.Play();
            return false;
        }
        return true;

    }

    public void DecrementWind()
    {
        if (PlayErrorFalse(WindPoints > 0))
        {
            SetWindBar(--WindPoints);
            UnassignedPoints++;
            menuChange.Play();
            SetUnassignedBar();
        }
    }

    public void DecrementIce()
    {
        if (PlayErrorFalse(IcePoints > 0))
        {
            SetIceBar(--IcePoints);
            UnassignedPoints++;
            menuChange.Play();
            SetUnassignedBar();
        }
    }

    public void DecrementFire()
    {
        if (PlayErrorFalse(FirePoints > 0))
        {
            SetFireBar(--FirePoints);
            UnassignedPoints++;
            menuChange.Play();
            SetUnassignedBar();
        }
    }

    public void IncrementWind()
    {
        if (PlayErrorFalse(EnoughPoints() && WindPoints < LevelCutoff3))
        {
            SetWindBar(++WindPoints);
            UnassignedPoints--;
            menuChange.Play();
            SetUnassignedBar();
        }
    }

    public void IncrementIce()
    {
        if (PlayErrorFalse(EnoughPoints() && IcePoints < LevelCutoff3))
        {
            SetIceBar(++IcePoints);
            UnassignedPoints--;
            menuChange.Play();
            SetUnassignedBar();
        }
    }

    public void IncrementFire()
    {
        if (PlayErrorFalse(EnoughPoints() && FirePoints < LevelCutoff3))
        {
            SetFireBar(++FirePoints);
            UnassignedPoints--;
            menuChange.Play();
            SetUnassignedBar();
        }
    }


    private bool EnoughPoints()
    {
        if (UnassignedPoints <= 0)
        {
            menuError.Play();
            return false; ;
        }
        
        return true;
    }

    
    public void OnButtonClose()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1;
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



  

   
}
