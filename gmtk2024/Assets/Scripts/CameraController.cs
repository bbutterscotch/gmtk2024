using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraController : MonoBehaviour
{

    private MapController mc;
    private Tilemap tilemap;
    private Camera camera;

    // Start is called before the first frame update
    void Start()
    {
        mc = FindFirstObjectByType<MapController>();
        tilemap = mc.walkable;
        camera = GetComponent<Camera>();
        Vector3 newPos = tilemap.CellToWorld(mc.center);
        newPos.z = -10;
        transform.position = newPos;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
