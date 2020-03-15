using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTotemController : EnemyController
{
    private float cooldownTimer = 0f;
    private float cooldownTime = 2f;
    private bool isCooldown;

    public CyclopsBossController cyclops;  // these are used only for cyclops fight
    public int totemNum;

    private float maxTotemRange = 20f;

    private int raycastLayerMask;

    public GameObject explosiveAttackPrefab;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        raycastLayerMask =  ((1 << LayerMask.NameToLayer("Obstacles")) |
                        (1 << LayerMask.NameToLayer("Walls")) |
                        (1 << LayerMask.NameToLayer("Player")));
    }

    // Update is called once per frame
    public override void Update()
    {
        if(player.transform.position.x < transform.position.x)
            transform.localScale = new Vector3(-8, 8, 1);
        else
            transform.localScale = new Vector3(8, 8, 1);

        if(isCooldown) {
            cooldownTimer += Time.deltaTime;
            if(cooldownTimer > cooldownTime) {
                isCooldown = false;
                cooldownTimer = 0f;
            }
        }
        else if(CheckLineOfSight()) {
            launchExplosiveAttack();
        }
        
    }

    public void handleAttack() {
        Vector3 pos = transform.position + new Vector3(0f, 0.3f, 0);
        Quaternion q = Quaternion.Euler( 0f, 0f, 0f );

        GameObject thisAttack = Instantiate(explosiveAttackPrefab, pos, q);
        thisAttack.GetComponent<EnemyController>().player = player;

        an.SetBool("isAttack", false);
    }

    public override void handleEnemyDeath() {
        base.handleEnemyDeath();
        if(cyclops) {
            cyclops.handleTotemDeath(totemNum);
        }
        Destroy(gameObject);
    }

    public void launchExplosiveAttack() {
        an.SetBool("isAttack", true);
        isCooldown = true;
    }

    // returns whether the player is in LoS of the ArcherBoy
    private bool CheckLineOfSight() {
        bool isAllHit = true;

        if(Vector3.Distance(player.transform.position, transform.position) < maxTotemRange) {
            Vector3[] rayStartingPoints = {
                transform.position + new Vector3(0.5f, 0, 0),
                transform.position + new Vector3(-0.5f, 0, 0),
                transform.position + new Vector3(0, 0.5f, 0),
                transform.position + new Vector3(0, -0.5f, 0)
            };
            foreach(Vector3 initialPos in rayStartingPoints) {
                Vector3 rayDirection = player.transform.position - initialPos;
                RaycastHit2D hit = Physics2D.Raycast(initialPos, rayDirection, maxTotemRange, raycastLayerMask);
                if(!hit || hit.transform != player.transform)
                    isAllHit = false;
            }

            return isAllHit;
        }
        return false;
    }
}
