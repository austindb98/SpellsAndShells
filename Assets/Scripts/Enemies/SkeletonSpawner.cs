using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class SkeletonSpawner : MonoBehaviour
{
    public Animator an;
    public GameObject skeletonWarrior;
    public GameObject skeletonArcher;
    public SkeletonKingController skeletonKingController;

    private bool isSpawning = false;
    private float spawnInterval = 0.7f;
    private float spawnTime = 3.7f;
    private float spawnTimer = 0f;

    private System.Random rnd;

    // Start is called before the first frame update
    void Start()
    {
        rnd = new System.Random();
    }

    // Update is called once per frame
    void Update()
    {
        if(isSpawning) {
            spawnTimer += Time.deltaTime;
            if(spawnTimer > spawnInterval) {
                if(rnd.Next(0, 3) == 0)
                    Instantiate(skeletonArcher, transform.position + new Vector3(-1, 0, 0), new Quaternion(0, 0, 0, 0));
                else
                    Instantiate(skeletonWarrior, transform.position + new Vector3(-1, 0, 0), new Quaternion(0, 0, 0, 0));
                skeletonKingController.handleSpawnSkeleton();
                spawnTimer -= spawnInterval;
                spawnTime -= spawnInterval;
            }
            if(spawnTimer > spawnTime) {
                closePortal();
            }
        }
    }

    public void handlePortalOpen() {
        an.SetBool("isPortalOpen", true);
        isSpawning = true;
    }

    public void handlePortalClose() {
        Destroy(gameObject);
    }

    public void closePortal() {
        an.SetBool("isPortalOpen", false);
        isSpawning = false;
    }
}

