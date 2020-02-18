using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ObstacleController : MonoBehaviour
{
    private GameObject soundManager;
    public AudioClip breakSound;
    public GameObject[] drops;
    public int dropChance;
    // Start is called before the first frame update
    void Start()
    {
        soundManager = GameObject.Find("SoundManager");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Break(Vector3 pos) {
        Tilemap map = GetComponent<Tilemap>();
        Vector3Int tilePos = GetComponent<GridLayout>().WorldToCell(pos);
        map.SetTile(tilePos, null);
        soundManager.GetComponent<AudioSource>().PlayOneShot(breakSound);
        float drop = Random.Range(0, drops.Length * dropChance);
        if(drop < drops.Length) {
            Instantiate(drops[(int)drop], pos, Quaternion.identity);
        }
    }
}
