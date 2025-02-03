using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using FMODUnity;
public class BirdController : MonoBehaviour
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
    [SerializeField] private EventReference beeDeathSound;
    PathFinder4 pf;
    private System.Random rand;
    private float delay = 1f;
    private int moves = 0;
    private int currIndex;

    private void Awake()
    {
        mc = FindFirstObjectByType<MapController>();
        hv = FindFirstObjectByType<HiveResources>();
        pf = FindFirstObjectByType<PathFinder4>();
        tilemap = mc.walkable;
        rand = new System.Random();
    }

    // Start is called before the first frame update
    void Start()
    {
        currIndex = pf.path.FindIndex(x => x == tilemap.WorldToCell(new Vector3(transform.position.x, transform.position.y, 0)));
        StartCoroutine(moveTile());
    }

    IEnumerator moveTile()
    {
        yield return new WaitForSeconds(delay);
        if (moves == 6)
        {
            Destroy(gameObject);
        }
        int up = rand.Next(0, 2);
        int nextIndex = currIndex;
        if (up == 0)
        {
            nextIndex = currIndex + 1;
        } else
        {
            nextIndex = currIndex - 1;
        }
        if (nextIndex < 0)
        {
            nextIndex = pf.path.Count - 1;
        }
        if (nextIndex >= pf.path.Count)
        {
            nextIndex = 0;
        }
        Vector3 distance = tilemap.CellToWorld(pf.path[nextIndex]) - new Vector3Int(0, 0, 1) - transform.position;
        StartCoroutine(moveOverTime(distance));
        currIndex = nextIndex;
        moves += 1;
    }

    IEnumerator moveOverTime(Vector3 distance)
    {
        for (int i = 0; i < 12; i++)
        {
            transform.position += distance / 12;
            yield return new WaitForSeconds((1.0f / 24.0f));
        }
        StartCoroutine(moveTile());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        beesKilled++;
        AudioController.instance.PlayOneShot(beeDeathSound, this.transform.position);
        if (beesKilled == beesToKill)
        {
            Destroy(gameObject);
        }
    }
}
