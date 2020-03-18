using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class CyclopsBossController : EnemyController
{
    private enum CyclopsState {
        Idle, Summon, Laser, Flee, PlantTotems, RunToCenter, LaserSnakes
    }
    private float summonTimer = 0f;
    private float summonTime = 4.5f;
    private float summonInterval = 1.5f;

    private float maxLaserRange = 20f;

    private CyclopsState state = CyclopsState.Idle;

    public GameObject molePrefab;
    public GameObject explosiveAttackPrefab;

    private Vector3[] enemySpawnDirections;

    private bool[] activeTotems = {false, false, false, false};
    public GameObject totemPrefab;
    public List<GameObject> totemPlantList;
    private int currentTotemPlantIndex = -1;
    private bool isPlanting = false;

    public GameObject roomCenter;

    private float laserTimer = 0f;
    private float laserTime = 12f;

    private float laserSnakesTimer = 0f;
    private float laserSnakesTime = 12f;
    private float laserSnakesInterval = 0.12f;
    private float laserSnakesAngleCounter = 0f;

    public RectTransform healthBar;

    private int raycastLayerMask;

    private System.Random rnd;

    private int nextStateInt = 0;

    private bool isLaserFinished;

    private float maxHealth;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        rnd = new System.Random();
        raycastLayerMask =  ((1 << LayerMask.NameToLayer("Obstacles")) |
                        (1 << LayerMask.NameToLayer("Walls")) |
                        (1 << LayerMask.NameToLayer("Player")));

        enemySpawnDirections = new Vector3[] {
            new Vector3(-1.5f, 1f, 0),
            new Vector3(1.5f, 1f, 0),
            new Vector3(0, -2f, 0)
        };
        maxHealth = enemyHealth.maxHealth;

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
                if(isLaserFinished)
                    return;

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
                    an.SetBool("isLaserSnakes", false);
                    isLaserFinished = true;
                    if(nextStateInt == 0)
                        transitionToPlantTotems();
                    else if(nextStateInt == 1)
                        transitionToLaserSnakes();
                    else
                        transitionToSummonState();
                    nextStateInt++;
                    if(nextStateInt > 2)
                        nextStateInt = 0;
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
                    an.SetInteger("numStomps", 0);
                    rb2d.constraints &= ~(RigidbodyConstraints2D.FreezePosition);
                    transitionToLaserState();
                }
                break;
            case CyclopsState.PlantTotems:
                if(currentTotemPlantIndex == -1) {
                    currentTotemPlantIndex = 0;

                    while(currentTotemPlantIndex < totemPlantList.Count && activeTotems[currentTotemPlantIndex])
                        currentTotemPlantIndex++;
                    if(currentTotemPlantIndex < totemPlantList.Count)
                        aiPath.target = totemPlantList[currentTotemPlantIndex].transform;
                    else {
                        currentTotemPlantIndex = -2;
                        return;
                    }

                    aiPath.target = totemPlantList[currentTotemPlantIndex].transform;
                    aiPath.canMove = true;
                }
                else if(currentTotemPlantIndex == -2) {
                    transitionToRunToCenter();
                }
                else if(!isPlanting && (aiPath.target.position - transform.position).magnitude < 2.5f) {
                    StartPlantTotem();
                }
                an.SetBool("isFacingRight", aiPath.desiredVelocity.x > 0);
                break;
            case CyclopsState.RunToCenter:
                if((aiPath.target.position - transform.position).magnitude < 1f) {
                    transitionToLaserState();
                }
                break;
            case CyclopsState.LaserSnakes:
                float[] angles = {0f, 120f, 240f};
                float angleOffset;
                laserSnakesTimer += Time.deltaTime;
                laserSnakesAngleCounter += Time.deltaTime;
                if(laserSnakesTimer < laserSnakesTime) {
                    if(laserSnakesTimer > laserSnakesInterval) {
                        angleOffset = laserSnakesAngleCounter / 3f * 360f;
                        foreach(float angle in angles) {
                            SpawnFireball(angle + angleOffset, 0f, 0f);
                        }
                        laserSnakesTimer -= laserSnakesInterval;
                        laserSnakesTime -= laserSnakesInterval;
                    }
                }
                else {
                    an.SetBool("isLaserSnakes", false);
                    transitionToLaserState();
                }
                break;
        }
    }

    private void transitionToLaserSnakes() {
        an.SetBool("isLaserSnakes", true);
        an.SetBool("isWalking", false);
        aiPath.canMove = false;
        laserSnakesTimer = 0f;
        laserSnakesTime = 12f;
        laserSnakesAngleCounter = 0f;
        rb2d.constraints |= RigidbodyConstraints2D.FreezePosition;
    }

    public void handleLaserSnakes() {
        state = CyclopsState.LaserSnakes;
    }

    public override void handleEnemyDeath() {
        base.handleEnemyDeath();
        Destroy(gameObject);
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
        SetHealth(enemyHealth.currentHealth - enemyHealth.calculateDamageTaken(shotgunDamage, BaseAttack.Element.Normal));
        enemyHealth.takeDamage(shotgunDamage, BaseAttack.Element.Normal);
    }

    public override void handleAttack(float damage, BaseAttack.Element element) {
        SetHealth(enemyHealth.currentHealth - enemyHealth.calculateDamageTaken(damage, element));
        enemyHealth.takeDamage(damage, element);
    }

    private void transitionToSummonState() {
        an.SetBool("isSummon", true);
        an.SetInteger("numStomps", 0);
        aiPath.canMove = false;
        summonTimer = 0f;
        summonTime = 4.5f;
        rb2d.constraints |= RigidbodyConstraints2D.FreezePosition;
    }

    private void transitionToLaserState() {
        state = CyclopsState.Laser;
        laserTimer = 0f;
        laserTime = 12f;
        isLaserFinished = false;
    }

    private void transitionToRunToCenter() {
        aiPath.canMove = true;
        aiPath.target = roomCenter.transform;
        rb2d.constraints &= ~RigidbodyConstraints2D.FreezePosition;
        state = CyclopsState.RunToCenter;
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
        // increment until next available
        while(currentTotemPlantIndex < totemPlantList.Count && activeTotems[currentTotemPlantIndex])
            currentTotemPlantIndex++;
        if(currentTotemPlantIndex < totemPlantList.Count)
            aiPath.target = totemPlantList[currentTotemPlantIndex].transform;
        else
            currentTotemPlantIndex = -2;
        isPlanting = false;
    }

    public void PlantTotem() {
        FireTotemController ftc = Instantiate(totemPrefab, aiPath.target.position, new Quaternion(0f, 0f, 0f, 0f))
                                    .GetComponent<FireTotemController>();
        ftc.cyclops = this;
        ftc.totemNum = currentTotemPlantIndex;
        activeTotems[currentTotemPlantIndex] = true;
        an.SetBool("isPlantingTotem", false);
    }

    public void handleTotemDeath(int totemNum) {
        activeTotems[totemNum] = false;
    }

    private void SummonEnemies() {
        foreach(Vector3 spawnVec in enemySpawnDirections) {
            SpawnMole(spawnVec);
        }
    }

    private void transitionToPlantTotems() {
        currentTotemPlantIndex = -1;
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

    public void SetHealth(float newHealth)
    {
        if (newHealth < 0)
            newHealth = 0;
        healthBar.localScale = new Vector3(newHealth / maxHealth, 1, 1);
    }
}
