using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using FMODUnity;

public class BeeSpawner : MonoBehaviour
{

    [SerializeField] GameObject beePrefab;
    private MapController mc;
    private Tilemap tilemap;
    private HiveResources hv;
    [SerializeField] List<Sprite> beeTypes;

    private int beemony = 0;
    [SerializeField] private EventReference beeSpawnSound;
    [SerializeField] private EventReference music;

    // Start is called before the first frame update
    void Start()
    {
        hv = FindObjectOfType<HiveResources>();
        mc = FindObjectOfType<MapController>();
        tilemap = mc.spriteMap;
    }


    public void SpawnBee()
    {
        hv.bees++;
        beemony += 5;
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Beemony", beemony);
        GameObject newBee = Instantiate(beePrefab, tilemap.CellToWorld(mc.startTile), Quaternion.identity);
        SpriteRenderer newSprite = newBee.GetComponent<SpriteRenderer>();
        newSprite.sprite = beeTypes[Random.Range(0, beeTypes.Count)];
        AudioController.instance.PlayOneShot(beeSpawnSound, this.transform.position);
    }
}
