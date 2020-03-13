using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class CyclopsBossController : EnemyController
{
    private enum CyclopsState {
        Idle, Summon, Laser, Flee, FireCircle, PlantTotems
    }
    private float summonTimer = 0f;
    private float summonTime = 4.5f;
    private float summonInterval = 1.5f;

    private CyclopsState state = CyclopsState.Idle;

    public GameObject molePrefab;
    public GameObject explosiveAttackPrefab;

    private Vector3[] enemySpawnDirections;

    public GameObject totemPrefab;
    public List<GameObject> totemPlantList;
    private int currentTotemPlantIndex = -1;
    private bool isPlanting = false;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        enemySpawnDirections = new Vector3[] {
            new Vector3(-1.5f, 1f, 0),
            new Vector3(1.5f, 1f, 0),
            new Vector3(0, -2f, 0)
        };

        //state = CyclopsState.Laser;
        //an.SetBool("isLaser", true);

        //state = CyclopsState.Summon;
        an.SetBool("isSummon", true);
        aiPath.canMove = false;
        //PlantTotems();
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
                    StartFireCircle();
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
            case CyclopsState.PlantTotems:
                if(currentTotemPlantIndex == -1) {
                    currentTotemPlantIndex = 0;
                    aiPath.target = totemPlantList[currentTotemPlantIndex].transform;
                    aiPath.canMove = true;
                }
                else if(!isPlanting && (aiPath.target.position - transform.position).magnitude < 1f) {
                    StartPlantTotem();
                }
                an.SetBool("isFacingRight", aiPath.desiredVelocity.x > 0);
                break;
        }
    }

    private void StartPlantTotem() {
        aiPath.canMove = false;
        an.SetBool("isWalking", false);
        an.SetBool("isPlantingTotem", true);
        isPlanting = true;
    }

    public void handleFinishPlantTotem() {
        an.SetBool("isWalking", true);
        aiPath.canMove = true;
        currentTotemPlantIndex++;
        aiPath.target = totemPlantList[currentTotemPlantIndex].transform;
        isPlanting = false;
    }

    public void PlantTotem() {
        Instantiate(totemPrefab, aiPath.target.position, new Quaternion(0f, 0f, 0f, 0f));
        an.SetBool("isPlantingTotem", false);
    }

    private void SummonEnemies() {
        foreach(Vector3 spawnVec in enemySpawnDirections) {
            SpawnMole(spawnVec);
        }
    }

    private void PlantTotems() {
        state = CyclopsState.PlantTotems;
        an.SetBool("isWalking", true);
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
        float[] angles = {-10f, -5f, 0f, 5f, 10f};
        foreach(float angle in angles) {
            SpawnFireball(angle, 0f, 0f);
        }
    }

    public void handleStomp() {
        an.SetInteger("numStomps", an.GetInteger("numStomps") + 1); // increment stomp counter
    }

    public void handleSummon() {
        state = CyclopsState.Summon;
        StartFireCircle();
    }

    public void handleFinishSummon() {
        state = CyclopsState.Idle;
        an.SetBool("isIdle", true);
    }

    public void StartFireCircle() {
        int numFireballs = 30;
        int fireballIndex = numFireballs;
        float stoppingTime = 0.85f;
        float relaunchMin = 1f;
        float relaunchMax = 2f;

        while(fireballIndex-- > 0) {
            SpawnFireball((float) fireballIndex * 360f / numFireballs, stoppingTime,
                    (float) fireballIndex / (float) numFireballs * (relaunchMax - relaunchMin) + relaunchMin);
        }
    }

    private void SpawnFireball(float angle, float stoppingTime, float relaunchTime) {
        Vector3 explosiveLaunchPos = transform.position + new Vector3(-.2f, 1.4f, 0);
        Quaternion q = Quaternion.Euler( 0f, 0f, 0f );

        GameObject thisAttack = Instantiate(explosiveAttackPrefab, explosiveLaunchPos, q);
        thisAttack.GetComponent<EnemyController>().player = player;
        CyclopsExplosiveAttackController ctrl = thisAttack.GetComponent<CyclopsExplosiveAttackController>();
        ctrl.angle = angle;
        ctrl.stoppingTime = stoppingTime;
        ctrl.relaunchTime = relaunchTime;
    }
}
