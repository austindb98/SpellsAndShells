using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraZoom : MonoBehaviour
{

    // The most zoomed out
    public float zoomOutSize = 20f;
    public float zoomInSize = 10f;

    static float ZoomSpeed = 12f;

    private Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
        cam.orthographicSize = zoomOutSize;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("ZoomOut"))
        {
            if (cam.orthographicSize < zoomOutSize)
            {
                cam.orthographicSize += ZoomSpeed * Time.deltaTime;
            }

        } else // need to zoom in
        {
            if (cam.orthographicSize > zoomInSize)
            {
                cam.orthographicSize -= ZoomSpeed * Time.deltaTime;
            }
        }
    }
}
