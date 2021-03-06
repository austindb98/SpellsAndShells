﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalPortalController : MonoBehaviour
{
    public Animator an;
    public GameObject mendohlPrefab;
    public FinalSceneController sceneController;

    private float spawnMendohlTime = 3f;
    private float spawnMendohlTimer = 0f;
    private bool isMendohlSpawn = false;

    private float destroyMendohlTime = 2f;
    private float destroyMendohlTimer = 0f;
    private bool isMendohlDestroyed = false;

    private float destroyPortalTime = 2f;
    private float destroyPortalTimer = 0f;
    private bool isPortalDestroyed = false;

    private bool isDialogFinished = false;

    public bool isDisappear = false;
    private bool isPortalClosed = false;

    public GameObject mendohl;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!isMendohlSpawn) {
            spawnMendohlTimer += Time.deltaTime;
            if(spawnMendohlTimer > spawnMendohlTime) {
                Quaternion q = Quaternion.Euler( 0f, 0f, 0f );
                mendohl = Instantiate(mendohlPrefab, transform.position + new Vector3(2.5f, -.3f, 0), q);
                sceneController.handleMendohlSpawn();
                isMendohlSpawn = true;
            }
        }
        else if(!isDisappear && !isMendohlDestroyed) {
            if(!DialogScheduler.HasDialog() && !isMendohlDestroyed && !isDialogFinished) {
                sceneController.handleFinishDialog();
                isDialogFinished = true;
            }
        }
        else if(isDisappear) {
            if(!DialogScheduler.HasDialog() && !isMendohlDestroyed) {
                destroyMendohlTimer += Time.deltaTime;
                if(destroyMendohlTimer > destroyMendohlTime) {
                    Destroy(mendohl);
                    isMendohlDestroyed = true;
                    isDisappear = false;
                }
            }
        }
        else if(!isPortalClosed) {
            destroyPortalTimer += Time.deltaTime;
            if(destroyPortalTimer > destroyPortalTime) {
                closePortal();
            }
        }
    }

    public void handlePortalOpen() {
        an.SetBool("isPortalOpen", true);
    }

    public void handlePortalClose() {
        sceneController.handlePortalClose();
        Destroy(gameObject);
    }

    public void closePortal() {
        an.SetBool("isPortalOpen", false);
        sceneController.SpawnSkeletons();
        isPortalClosed = true;
    }
}

