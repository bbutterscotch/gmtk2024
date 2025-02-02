using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

public class cameraControl : MonoBehaviour
{
    [SerializeField] int minZoom = 3;
    [SerializeField] int maxZoom = 20;
    [SerializeField] int zoomSpeed = 2;
    [SerializeField] TMP_Text mouseLocation;
    MapController mc;
    Tilemap tm;

    Vector3 mouseWorldPosStart;

    private void Awake()
    {
        mc = FindFirstObjectByType<MapController>();
        tm = mc.walkable;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(2))
        {
            //Debug.Log("Middle Mouse Button");
            mouseWorldPosStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        if (Input.GetMouseButton(2))
        {
            Pan();
        }

        Zoom(Input.GetAxis("Mouse ScrollWheel"));
        Vector3Int loc = tm.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        mouseLocation.text = "(" + loc.x.ToString() + ", " + loc.y.ToString() + ")";

    }

    private void Pan()
    {
        if (Input.GetAxis("Mouse Y") != 0 || Input.GetAxis("Mouse X") != 0)
        {
            Vector3 mouseWorldPosDiff = mouseWorldPosStart - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position += mouseWorldPosDiff;
        }
    }

    private void Zoom(float zoomDiff)
    {
        if (zoomDiff != 0)
        {
            Vector3 mouseWorldPosStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - zoomDiff * zoomSpeed, minZoom, maxZoom);
            Vector3 mouseWorldPosDiff = mouseWorldPosStart - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position += mouseWorldPosDiff;
        }
    }
}
