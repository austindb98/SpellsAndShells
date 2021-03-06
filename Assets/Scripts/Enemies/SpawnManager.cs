﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Pathfinding;

public class SpawnManager : MonoBehaviour
{
    // Start is called before the first frame update
    private float spawnMinRadius = 1f;
    private float spawnMaxRadius = 2f;
    private float avgSpawnRate = 6f;
    private bool isActive = false;

    private int numSpawnedEnemies = 0;

    public int initialSpawnNum = 3;

    private System.Random rnd;
    public float spawnSlope = -.13f;
    public float spawnIntercept = 1f;

    public GameObject enemyPrefab;
    public GameObject player;

    private SpawnMaster spawnMaster;

    void Start()
    {
        rnd = new System.Random();
        player = GameObject.FindWithTag("Player");
        spawnMaster = gameObject.GetComponent<EnemyController>().spawnMaster;
    }

    // Update is called once per frame
    void Update()
    {
        float relativeTimePassed = Time.deltaTime / avgSpawnRate;

        if(!isActive && spawnMaster.isActive) {
            isActive = true;
            int i = initialSpawnNum;
            while(i-- > 0) {
                SpawnEnemy();
            }
        }

        if(isActive && rnd.NextDouble() < Time.deltaTime * (spawnIntercept + spawnSlope * numSpawnedEnemies)) {
            SpawnEnemy();
        }
    }

    void SpawnEnemy() {
        float spawnRadius;
        float spawnAngle;

        spawnRadius = rnd.Next((int) (spawnMinRadius * 1000), (int) (spawnMaxRadius * 1000)) / 1000f;
        spawnAngle = rnd.Next(0, 360);

        Vector3 spawnVec = new Vector3(Mathf.Cos(spawnAngle), Mathf.Sin(spawnAngle), 0) * spawnRadius;
        GameObject thisEnemy = Instantiate(enemyPrefab, transform.position + spawnVec, Quaternion.Euler( 0f, 0f, 0f ));
        AIDestinationSetter destinationSetter = thisEnemy.GetComponent<AIDestinationSetter>();
        EnemyController enemyController = thisEnemy.GetComponent<EnemyController>();

        spawnMaster.addEnemyToList(enemyController);
        enemyController.player = player.gameObject;
        enemyController.spawnManager = this;
        destinationSetter.target = player.transform;
        numSpawnedEnemies++;
    }

    public void decrementEnemyCounter() {
        numSpawnedEnemies--;
    }

}
