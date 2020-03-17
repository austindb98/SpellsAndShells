using System;
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Pathfinding;

public class ObstacleController : MonoBehaviour
{
    public GameObject[] drops;
    public float dropChance;
    public GridGraph graphToScan;
    private HashSet<Tuple<Vector3,bool>> breakList;
    public AudioClip breakSound;
    private Tilemap map;

    // Start is called before the first frame update
    void Start()
    {
        graphToScan = AstarPath.active?.data.gridGraph;
        breakList = new HashSet<Tuple<Vector3,bool>>();
        map = GetComponent<Tilemap>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void LateUpdate() {
        if(breakList.Count > 0) {
            foreach(Tuple<Vector3,bool> tile in breakList) {

                //Tilemap map = GetComponent<Tilemap>();
                Vector3Int tilePos = transform.parent.GetComponentInParent<GridLayout>().WorldToCell(tile.Item1);
                map.SetTile(tilePos, null);

                Debug.Log("Destroying obstacle at: " + tilePos.x + ", " + tilePos.y);

                SoundController.playBreakSound(breakSound);
                if(tile.Item2) {
                    dropChance = Mathf.Clamp(dropChance,0,1);
                    float drop = UnityEngine.Random.Range(0, drops.Length / dropChance);
                    if(drop < drops.Length) {
                        // scale the drop position with the grid scale
                        Instantiate(drops[(int)drop], tile.Item1 * this.transform.localScale.z, Quaternion.identity);
                    }
                }
            }
            breakList = new HashSet<Tuple<Vector3,bool>>();
        }
    }


    //Does not drop items
    public void Break(Vector3 pos) {
        //bool isContains = false;

        Vector3Int tilePos = GetComponent<GridLayout>().WorldToCell(pos);
        breakList.Add(new Tuple<Vector3,bool>(tilePos,false));
        /*
        foreach(Vector3Int point in breakList) {
            if(point.x == tilePos.x && point.y == tilePos.y)
                isContains = true;
        }
        if(!isContains)
            breakList.Add(tilePos);
        */

    }

    //Drops items
    public void PlayerBreak(Vector3 pos) {
        Vector3Int tilePos = GetComponent<GridLayout>().WorldToCell(pos);
        breakList.Add(new Tuple<Vector3,bool>(tilePos,true));
    }
}
