using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiveResources : MonoBehaviour
{

    public int nectar = 0;
    public int pollen = 0;
    public int honey = 0;
    public int wax = 0;
    public int royalJelly = 0;

    public int bees = 0;
    
    public int pondTiles = 0;
    public int meadowTiles = 0;
    public int woodlandTiles = 0;
    public int beekeeperTiles = 0;
    public int gardenTiles = 0;
    public int forestTiles = 0;
    public int apiaryTiles = 0;
    public int parkTiles = 0;

    public int nurseryTiles = 0;

    public int[] pondCost = { 4, 0, 0, 0, 1 };
    public int[] meadowCost = { 0, 2, 3, 0, 0 };
    public int[] beekeeperCost = { 0, 0, 2, 0, 3 };
    public int[] woodlandCost = { 1, 0, 2, 0, 2 };
    public int[] gardenCost = { 0, 5, 0, 0, 0 };

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool BuyTile(string tileName)
    {
        int[] cost = { 0, 0, 0, 0, 0 };
        Debug.Log(tileName);
        if (tileName == "Tile_Pond_Drop") { cost = pondCost; }
        else if (tileName == "Tile_Meadow_Drop") { cost = meadowCost; }
        else if (tileName == "Tile_Beekeeper_Drop") { cost = beekeeperCost; }
        else if (tileName == "Tile_Woodland_Drop") { cost = woodlandCost; }
        else if (tileName == "Tile_Garden_Drop") { cost = gardenCost; }
        else { return false; }
        if (cost[0] != 0)
        {
            if (honey >= cost[0])
            {
                honey -= cost[0];
            }
            else
            {
                return false;
            }
        }
        if (cost[1] != 0)
        {
            if (pollen >= cost[1])
            {
                pollen -= cost[1];
            }
            else
            {
                return false;
            }
        }
        if (cost[2] != 0)
        {
            if (nectar >= cost[2])
            {
                nectar -= cost[2];
            }
            else
            {
                return false;
            }
        }
        if (cost[3] != 0)
        {
            if (wax >= cost[3])
            {
                wax -= cost[3];
            }
            else
            {
                return false;
            }
        }
        if (cost[4] != 0)
        {
            if (royalJelly >= cost[4])
            {
                royalJelly -= cost[4];
            }
            else
            {
                return false;
            }
        }
        return true;
    }
}
