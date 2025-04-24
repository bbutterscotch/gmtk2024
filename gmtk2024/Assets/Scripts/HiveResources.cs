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

    public int nurseryTiles = 1;
    public int armoryTiles = 0;
    public int honeySuperTiles = 0;

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
        if (tileName == "Tile_Pond_Drop" && honey >= 4 && wax >= 1) 
        {
            honey -= 4;
            wax -= 1;
        }
        else if (tileName == "Tile_Meadow_Drop" && nectar >= 3 && pollen >= 2) 
        {
            nectar -= 3;
            pollen -= 2;
        }
        else if (tileName == "Tile_Beekeeper_Drop" && wax >= 3 && nectar >= 2) 
        {
            wax -= 3;
            nectar -= 2;
        }
        else if (tileName == "Tile_Woodland_Drop" && wax >= 2 && nectar >= 2 && honey >= 1) 
        {
            wax -= 2;
            honey -= 1;
            nectar -= 2;
        }
        else if (tileName == "Tile_Garden_Drop" && pollen >= 5) 
        {
            pollen -= 5;
        }
        else if (tileName == "Tile_Nursery_Spawn" && wax >= 10 && pollen >= 5 && honey >= 5)
        {
            wax -= 10;
            pollen -= 5;
            honey -= 5;
            nurseryTiles++;
        } 
        else if (tileName == "Tile_HoneySuper_Spawn" && royalJelly >= 15 && honey >= 10 && nectar >= 5 && pollen >= 5)
        {
            royalJelly -= 15;
            honey -= 10;
            nectar -= 5;
            pollen -= 5;
            honeySuperTiles++;
        }
        else if (tileName == "Tile_Armory_Spawn" && royalJelly >= 8 && wax >= 5 && nectar >= 5) 
        {
            royalJelly -= 8;
            wax -= 5;
            nectar -= 5;
            armoryTiles++;
        }
        else 
        { 
            return false; 
        }

        return true;
    }
}
