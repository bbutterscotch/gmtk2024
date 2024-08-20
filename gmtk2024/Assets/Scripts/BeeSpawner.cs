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

    [SerializeField] private EventReference beeSpawnSound;

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
        GameObject newBee = Instantiate(beePrefab, tilemap.CellToWorld(mc.startTile), Quaternion.identity);
        SpriteRenderer newSprite = newBee.GetComponent<SpriteRenderer>();
        newSprite.sprite = beeTypes[Random.Range(0, beeTypes.Count)];
        AudioController.instance.PlayOneShot(beeSpawnSound, this.transform.position);
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Beemony", hv.bees);
    }
}
