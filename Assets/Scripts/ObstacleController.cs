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

    public void Break(Vector3 hit) {
        Tilemap tilemap = this.GetComponent<Tilemap>();
        Vector3Int cellPosition = tilemap.WorldToCell(hit);
        tilemap.SetTile(cellPosition, null);
        SoundController.playBreakSound(breakSound);
        float drop = Random.Range(0, drops.Length * dropChance);
        if (drop < drops.Length)
        {
            // scale the drop position with the grid scale
            Instantiate(drops[(int)drop], tilemap.GetCellCenterWorld(cellPosition) * this.transform.localScale.z, Quaternion.identity);
        }
    }
}
