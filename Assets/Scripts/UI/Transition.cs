using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class Transition : MonoBehaviour
{

    static readonly float transSpeed = .5f;

    public SceneDoor parent;

    private Image overlay;
    // Start is called before the first frame update
    void Start()
    {
        overlay = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (parent.IsLocked())
        {
            return;
        }
        if (overlay.color.a >= 1)
        {
            parent.HandleUnlocked();
        } else
        {
            Color c = overlay.color;
            c.a += Mathf.Min(Time.deltaTime * transSpeed, 1);
            overlay.color = c;
        }
    }
}
