using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class CyclopsBossController : EnemyController
{
    private enum CyclopsState {
        Idle, Summon, Laser, Flee, PlantTotems
    }
    private float summonTimer = 0f;
    private float summonTime = 4.5f;
    private float summonInterval = 1.5f;

    private float maxLaserRange = 20f;

    private CyclopsState state = CyclopsState.Idle;

    public GameObject molePrefab;
    public GameObject explosiveAttackPrefab;

    private Vector3[] enemySpawnDirections;

    public GameObject totemPrefab;
    public List<GameObject> totemPlantList;
    private int currentTotemPlantIndex = -1;
    private bool isPlanting = false;

    private float laserTimer = 0f;
    private float laserTime = 12f;
    
    private int raycastLayerMask;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        raycastLayerMask =  ((1 << LayerMask.NameToLayer("Obstacles")) |
                        (1 << LayerMask.NameToLayer("Walls")) |
                        (1 << LayerMask.NameToLayer("Player")));

        enemySpawnDirections = new Vector3[] {
            new Vector3(-1.5f, 1f, 0),
            new Vector3(1.5f, 1f, 0),
            new Vector3(0, -2f, 0)
        };

        transitionToSummonState();
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
            case CyclopsState.Laser:
                if(CheckLineOfSight()) {
                    an.SetBool("isWalking", false);
                    an.SetBool("isLaser", true);
                    aiPath.canMove = false;
                    rb2d.constraints |= RigidbodyConstraints2D.FreezePosition;
                }
                else {
                    aiPath.canMove = true;
                    rb2d.constraints &= ~RigidbodyConstraints2D.FreezePosition;
                    an.SetBool("isLaser", false);
                    an.SetBool("isWalking", true);
                }
                laserTimer += Time.deltaTime;
                if(laserTimer > laserTime) {
                    rb2d.constraints &= ~RigidbodyConstraints2D.FreezePosition;
                    an.SetBool("isLaser", false);
                    transitionToPlantTotems();
                }
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
                    rb2d.constraints &= ~(RigidbodyConstraints2D.FreezePosition);
                    transitionToLaserState();
                }
                break;
            case CyclopsState.PlantTotems:
                if(currentTotemPlantIndex == -1) {
                    currentTotemPlantIndex = 0;
                    aiPath.target = totemPlantList[currentTotemPlantIndex].transform;
                    aiPath.canMove = true;
                }
                else if(currentTotemPlantIndex == -2) {
                    transitionToLaserState();
                }
                else if(!isPlanting && (aiPath.target.position - transform.position).magnitude < 1f) {
                    StartPlantTotem();
                }
                an.SetBool("isFacingRight", aiPath.desiredVelocity.x > 0);
                break;
        }
    }

    // returns whether the player is in LoS of the ArcherBoy
    private bool CheckLineOfSight() {
        bool isAllHit = true;

        if(Vector3.Distance(player.transform.position, transform.position) < maxLaserRange) {
            Vector3[] rayStartingPoints = {
                transform.position + new Vector3(0.5f, 0, 0),
                transform.position + new Vector3(-0.5f, 0, 0),
                transform.position + new Vector3(0, 0.5f, 0),
                transform.position + new Vector3(0, -0.5f, 0)
            };
            foreach(Vector3 initialPos in rayStartingPoints) {
                Vector3 rayDirection = player.transform.position - initialPos;
                RaycastHit2D hit = Physics2D.Raycast(initialPos, rayDirection, maxLaserRange, raycastLayerMask);
                if(!hit || hit.transform != player.transform)
                    isAllHit = false;
            }

            return isAllHit;
        }
        return false;
    }

    public override void handleShotgunAttack(int shotgunDamage) {
        if(enemyHealth)
            enemyHealth.takeDamage(shotgunDamage, BaseAttack.Element.Normal);
    }

    private void transitionToSummonState() {
        an.SetBool("isSummon", true);
        aiPath.canMove = false;
        summonTimer = 0f;
        summonTime = 4.5f;
        rb2d.constraints |= RigidbodyConstraints2D.FreezePosition;
    }

    private void transitionToLaserState() {
        state = CyclopsState.Laser;
        laserTimer = 0f;
        laserTime = 15f;
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
        if(currentTotemPlantIndex < totemPlantList.Count)
            aiPath.target = totemPlantList[currentTotemPlantIndex].transform;
        else
            currentTotemPlantIndex = -2;
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

    private void transitionToPlantTotems() {
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
