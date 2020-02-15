using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Pathfinding;

public class TwoDirectionGraphicsController : MonoBehaviour
{
    private Animator an;
    public AIPath aiPath;

    // Start is called before the first frame update
    void Start()
    {
        an = an = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float x = aiPath.desiredVelocity.x;
        float y = aiPath.desiredVelocity.y;


        if (x == 0 && y == 0)
        {
            an.SetBool("isTreantWalking", false);
        }
        else if (x >= Math.Abs(y))
            WalkRight();
        else if (x <= -1 * Math.Abs(y))
            WalkLeft();
        else
        {
            print("animation oof");
        }
    }


    private void WalkLeft()
    {
        an.SetInteger("treantDirection", 4);
        an.SetBool("isTreantWalking", true);
    }

    private void WalkRight()
    {
        an.SetInteger("treantDirection", 2);
        an.SetBool("isTreantWalking", true);
    }
}
