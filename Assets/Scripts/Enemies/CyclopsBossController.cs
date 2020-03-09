using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class CyclopsBossController : EnemyController
{
    private enum CyclopsState {
        Idle, Summon, Laser, Flee
    }
    private float summonTimer = 0f;
    private float summonTime = 4.5f;
    private float summonInterval = 1.5f;

    private CyclopsState state = CyclopsState.Idle;

    public GameObject molePrefab;
    public GameObject explosiveAttackPrefab;

    private Vector3[] enemySpawnDirections;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        enemySpawnDirections = new Vector3[] {
            new Vector3(-1.5f, 1f, 0),
            new Vector3(1.5f, 1f, 0),
            new Vector3(0, -2f, 0)
        };

        /*state = CyclopsState.Laser;
        an.SetBool("isLaser", true);*/

        //state = CyclopsState.Summon;
        an.SetBool("isSummon", true);
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        float deltaX = player.transform.position.x - transform.position.x;

        switch(state) {
            case CyclopsState.Idle:
                an.SetBool("isFacingRight", deltaX > 0);
                break;

            case CyclopsState.Summon:
                summonTimer += Time.deltaTime;
                if(summonTimer > summonInterval) {
                    SummonEnemies();
                    summonTimer -= summonInterval;
                    summonTime -= summonInterval;
                }
                if(summonTimer > summonTime) {
                    an.SetBool("isSummon", false);
                    state = CyclopsState.Idle;
                    summonTimer = 0f;
                    summonTime = 4.5f;
                }
                break;
        }
    }

    private void SummonEnemies() {
        foreach(Vector3 spawnVec in enemySpawnDirections) {
            SpawnMole(spawnVec);
        }
    }

    private void SpawnMole(Vector3 spawnVec) {
        GameObject thisEnemy = Instantiate(molePrefab, transform.position + spawnVec, Quaternion.Euler( 0f, 0f, 0f ));
        AIDestinationSetter destinationSetter = thisEnemy.GetComponent<AIDestinationSetter>();
        EnemyController enemyController = thisEnemy.GetComponent<EnemyController>();

        //spawnMaster.addEnemyToList(enemyController);
        enemyController.player = player.gameObject;
        destinationSetter.target = player.transform;
    }

    public void handleLaunchExplosiveAttack() {
        Vector3 explosiveLaunchPos = transform.position + new Vector3(-.2f, 1.4f, 0);
        Quaternion q = Quaternion.Euler( 0f, 0f, 0f );

        GameObject thisAttack = Instantiate(explosiveAttackPrefab, explosiveLaunchPos, q);
        thisAttack.GetComponent<EnemyController>().player = player;
    }

    public void handleStomp() {
        an.SetInteger("numStomps", an.GetInteger("numStomps") + 1); // increment stomp counter
    }

    public void handleSummon() {
        state = CyclopsState.Summon;
    }

    public void handleFinishSummon() {
        state = CyclopsState.Idle;
        an.SetBool("isIdle", true);
    }
}
