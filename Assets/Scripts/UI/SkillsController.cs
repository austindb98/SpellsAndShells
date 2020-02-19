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
        SetElementBar(windFluids, WindPoints, windCounter);
        SetElementBar(iceFluids, IcePoints, iceCounter);
        SetElementBar(fireFluids, FirePoints, fireCounter);
    }

    private void SetElementBar(GameObject[] fluids, uint elementPoints, Text counter)
    {
        fluids[0].SetActive(false);
        fluids[1].SetActive(false);
        fluids[2].SetActive(false);
        fluids[3].SetActive(false);
        if (elementPoints >= LevelCutoff1)
        {
            fluids[0].SetActive(true);
            if (elementPoints >= LevelCutoff2)
            {
                fluids[1].SetActive(true);
                if (elementPoints >= LevelCutoff3)
                {
                    fluids[2].SetActive(true);
                    if (elementPoints >= LevelCutoff4)
                    {
                        fluids[3].SetActive(true);
                    }
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
            SetElementBar(windFluids, --WindPoints, windCounter);
            UnassignedPoints++;
            menuChange.Play();
            SetUnassignedBar();
        }
    }

    public void DecrementIce()
    {
        if (PlayErrorFalse(IcePoints > 0))
        {
            SetElementBar(iceFluids, --IcePoints, iceCounter);
            UnassignedPoints++;
            menuChange.Play();
            SetUnassignedBar();
        }
    }

    public void DecrementFire()
    {
        if (PlayErrorFalse(FirePoints > 0))
        {
            SetElementBar(fireFluids, --FirePoints, fireCounter);
            UnassignedPoints++;
            menuChange.Play();
            SetUnassignedBar();
        }
    }

    public void IncrementWind()
    {
        if (PlayErrorFalse(EnoughPoints() && WindPoints < LevelCutoff4))
        {
            SetElementBar(windFluids, ++WindPoints, windCounter);
            UnassignedPoints--;
            menuChange.Play();
            SetUnassignedBar();
        }
    }

    public void IncrementIce()
    {
        if (PlayErrorFalse(EnoughPoints() && IcePoints < LevelCutoff4))
        {
            SetElementBar(iceFluids, ++IcePoints, iceCounter);
            UnassignedPoints--;
            menuChange.Play();
            SetUnassignedBar();
        }
    }

    public void IncrementFire()
    {
        if (PlayErrorFalse(EnoughPoints() && FirePoints < LevelCutoff4))
        {
            SetElementBar(fireFluids, ++FirePoints, fireCounter);
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
