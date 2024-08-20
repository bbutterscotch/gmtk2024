using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraControl : MonoBehaviour
{
    int minZoom = 3;
    int maxZoom = 20;
    int zoomSpeed = 2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float zoom = Camera.main.orthographicSize;
        if (Input.GetAxis("Mouse ScrollWheel") > 0f) {
            // Zoom in
            //print("zoom in");
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f) {
            // Zoom out
           // print("zoom out");
        }

        zoom -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        zoom = Mathf.Clamp(zoom, minZoom, maxZoom);
        Camera.main.orthographicSize = zoom;
    }
}
