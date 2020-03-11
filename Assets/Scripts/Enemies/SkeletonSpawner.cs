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
        spawnTime = 1.5f + 2.2f * (float) rnd.NextDouble();
    }

    // Update is called once per frame
    void Update()
    {
        int numArchers = 0;
        if(isSpawning) {
            spawnTimer += Time.deltaTime;
            GameObject skel;
            if(spawnTimer > spawnInterval) {
                if(numArchers < 2 && rnd.Next(0, 3) == 0) {
                    skel = Instantiate(skeletonArcher, transform.position + new Vector3(-1, 0, 0), new Quaternion(0, 0, 0, 0));
                    skel.GetComponent<SkeletalArcherController>().skeletonKingController = skeletonKingController;
                    numArchers++;
                }
                else {
                    skel = Instantiate(skeletonWarrior, transform.position + new Vector3(-1, 0, 0), new Quaternion(0, 0, 0, 0));
                    skel.GetComponent<SkeletonWarriorController>().skeletonKingController = skeletonKingController;
                }

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

