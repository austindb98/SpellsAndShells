using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Pathfinding;

public class ObstacleController : MonoBehaviour
{
    public GameObject[] drops;
    public int dropChance;
    public GridGraph graphToScan;
    private List<Vector3Int> breakList;
    public AudioClip breakSound;

    // Start is called before the first frame update
    void Start()
    {
        graphToScan = AstarPath.active?.data.gridGraph;
        breakList = new List<Vector3Int>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void LateUpdate() {
        if(breakList.Count > 0) {
            foreach(Vector3 pos in breakList) {
                Tilemap map = GetComponent<Tilemap>();
                Vector3Int tilePos = transform.parent.GetComponentInParent<GridLayout>().WorldToCell(pos);
                map.SetTile(tilePos, null); 
                SoundController.playBreakSound(breakSound);
                float drop = Random.Range(0, drops.Length * dropChance);
                if(drop < drops.Length) {
                    // scale the drop position with the grid scale
                    Instantiate(drops[(int)drop], pos * this.transform.localScale.z, Quaternion.identity);
                }
            }
            breakList = new List<Vector3Int>();
        }
    }

    public void Break(Vector3 pos) {
        bool isContains = false;
        Tilemap map = GetComponent<Tilemap>();
        Vector3Int tilePos = GetComponent<GridLayout>().WorldToCell(pos);

        foreach(Vector3Int point in breakList) {
            if(point.x == tilePos.x && point.y == tilePos.y)
                isContains = true;
        }
        if(!isContains)
            breakList.Add(tilePos);
    }
}
