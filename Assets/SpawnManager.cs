using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class SpawnManager : MonoBehaviour
{
    // Start is called before the first frame update
    private float spawnMinRadius = 3f;
    private float spawnMaxRadius = 5f;
    private float spawnTimer = 0f;
    private float avgSpawnRate = 6f;
    private System.Random rnd;

    public GameObject treant;
    public GameObject player;

    void Start()
    {
        rnd = new System.Random();
    }

    // Update is called once per frame
    void Update()
    {
        float relativeTimePassed = Time.deltaTime / avgSpawnRate;
        float spawnRadius;
        float spawnAngle;

        if(rnd.Next(0, 1000) < relativeTimePassed * 1000) {
            spawnRadius = rnd.Next((int) (spawnMinRadius * 1000), (int) (spawnMaxRadius * 1000)) / 1000f;
            spawnAngle = rnd.Next(0, 360);

            Vector3 spawnVec = new Vector3(Mathf.Cos(spawnAngle), Mathf.Sin(spawnAngle), 0) * spawnRadius;
            Instantiate(treant, transform.position + spawnVec, Quaternion.Euler( 0f, 0f, 0f ));
            AIDestinationSetter destinationSetter = treant.GetComponent<AIDestinationSetter>();
            destinationSetter.target = player.transform;
        }
    }
}
