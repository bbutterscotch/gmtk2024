using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BearController : MonoBehaviour
{

    [SerializeField] private int beesToKill = 10;
    private int beesKilled = 0;
    private MapController mc;
    private Tilemap tilemap;
    [SerializeField] private float timeToCenter = 5f;
    [SerializeField] private int stepsToCenter = 10;
    [SerializeField] private float timeAtCenter = 2f;
    HiveResources hv;
    [SerializeField] private int stolenHoney = 10;


    // Start is called before the first frame update
    void Start()
    {
        mc = FindObjectOfType<MapController>();
        tilemap = mc.walkable;
        Vector3 distance = tilemap.CellToWorld(mc.center) + new Vector3Int(0, 0, -1) - transform.position;
        StartCoroutine(moveToHive(distance));
        hv = FindObjectOfType<HiveResources>();
    }

    IEnumerator moveToHive(Vector3 distance)
    {
        for (int i = 0; i < stepsToCenter; i++)
        {
            transform.position += distance / stepsToCenter;
            yield return new WaitForSeconds(timeToCenter/stepsToCenter);
        }
        yield return new WaitForSeconds(timeAtCenter);
        hv.honey -= stolenHoney;
        // end case here?
        if (hv.honey < 0)
        {
            hv.honey = 0;
        }
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        beesKilled++;
        if (beesKilled == beesToKill)
        {
            Destroy(gameObject);
        }
    }
}
