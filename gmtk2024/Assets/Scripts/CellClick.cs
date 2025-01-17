using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using FMODUnity;

public class CellClick : MonoBehaviour
{
    [Header("Tilemaps")]
    [Tooltip("Shows the user's current cursor position over the map")]
    public Tilemap selectTilemap;

    [Tooltip("The hive tilemap")]
    public Tilemap tilemap;

    [Space(10)]

    [Header("Selection")]
    public AnimatedTile selectedTile;
    public Tile blankTile;
    //public AnimatedTile blueTile;
    public Color validOverlay; // Green or white for no overlay
    public Color invalidOverlay; // Red
    public SpriteMask circleMask;

    [Space(10)]

    [Header("- Tiles -")]
    [Header("Basic Tiles")]
    public AnimatedTile pondTile;
    public AnimatedTile meadowTile;
    public AnimatedTile beekeeperTile;
    public AnimatedTile woodlandTile;
    public AnimatedTile gardenTile;

    [Header("Advanced Tiles")]
    public AnimatedTile forestTile; // 3+ woodland
    public AnimatedTile apiaryTile; // 3+ beekeepers
    public AnimatedTile parkTile; // 1+ gardens, 1+ meadows, 1+ pond

    Vector3Int tilemapPos;
    Vector3Int prevTilePos;

    public bool isPlacing = false;

    // Names
    private string pondTileName;
    private string meadowTileName;
    private string beekeeperTileName;
    private string woodlandTileName;
    private string gardenTileName;

    private string forestTileName;
    private string apiaryTileName;
    private string parkTileName;

    private HiveResources hv;
    private PathFinder3 pf;

    [SerializeField] private EventReference tileBasicSound;
    [SerializeField] private EventReference tileAdvancedSound;
    [SerializeField] private EventReference music;

    // Tile Neighbors
    private Vector3Int[] evenNeighbors = {
        new Vector3Int(1, 0, 0), new Vector3Int(0, 1, 0), new Vector3Int(-1, 1, 0),
        new Vector3Int(-1, 0, 0), new Vector3Int(-1, -1, 0), new Vector3Int(0, -1, 0)
    };
    private Vector3Int[] oddNeighbors = {
        new Vector3Int(1, 0, 0), new Vector3Int(1, 1, 0), new Vector3Int(0, 1, 0),
        new Vector3Int(-1, 0, 0), new Vector3Int(0, -1, 0), new Vector3Int(1, -1, 0)
    };

    // Returns an array containing the names of each of a given tile's 6 neighbors
    string[] checkNeighbors(Vector3Int position) {
        string[] neighbors = new string[6];

        if (position.y % 2 == 0) {
            // Even
            for (int i = 0; i < evenNeighbors.Length; i++) {
                Vector3Int temp = new Vector3Int(position.x + evenNeighbors[i].x, position.y + evenNeighbors[i].y, 0);

                AnimatedTile tile = tilemap.GetTile<AnimatedTile >(new Vector3Int(temp.x, temp.y, 0));
                if (tile != null) {
                    neighbors[i] = tile.name;
                } else {
                    neighbors[i] = "";
                }
            }
        } else {
            // Odd
            for (int i = 0; i < oddNeighbors.Length; i++) {
                Vector3Int temp = new Vector3Int(position.x + oddNeighbors[i].x, position.y + oddNeighbors[i].y, 0);

                AnimatedTile tile = tilemap.GetTile<AnimatedTile >(new Vector3Int(temp.x, temp.y, 0));
                if (tile != null) {
                    neighbors[i] = tile.name;
                } else {
                    neighbors[i] = "";
                }
            }
        }

        return neighbors;
    }

    // Retuns an array of grid coordinates for each of a given tile's 6 neighbors
    Vector3Int[] getNeighborCoords(Vector3Int position) {
        Vector3Int[] neighbors = new Vector3Int[6];

        if (position.y % 2 == 0) {
            // Even
            for (int i = 0; i < evenNeighbors.Length; i++) {
                Vector3Int temp = new Vector3Int(position.x + evenNeighbors[i].x, position.y + evenNeighbors[i].y, 0);
                neighbors[i] = temp;
            }
        } else {
            // Odd
            for (int i = 0; i < oddNeighbors.Length; i++) {
                Vector3Int temp = new Vector3Int(position.x + oddNeighbors[i].x, position.y + oddNeighbors[i].y, 0);
                neighbors[i] = temp;
            }
        }

        return neighbors;
    }

    void Start()
    {
        circleMask.enabled = false;
        matchingNeighbors = new List<Vector3Int>();
        hv = FindObjectOfType<HiveResources>();
        pf = FindObjectOfType<PathFinder3>();

        pondTileName = pondTile.name;
        meadowTileName = meadowTile.name;
        beekeeperTileName = beekeeperTile.name;
        woodlandTileName = woodlandTile.name;
        gardenTileName = gardenTile.name;

        forestTileName = forestTile.name;
        apiaryTileName = apiaryTile.name;
        parkTileName = parkTile.name;
    }

    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        tilemapPos = selectTilemap.WorldToCell(mousePos);

        // Move the cursor
        if (tilemapPos != prevTilePos) {
            selectTilemap.SetTile(new Vector3Int(tilemapPos.x, tilemapPos.y, 0), selectedTile);
            selectTilemap.SetTile(new Vector3Int(prevTilePos.x, prevTilePos.y, 0), blankTile);

            prevTilePos = tilemapPos;

            // Change overlay based on whether or not tile placement is allowed
            if (isPlacing) {
                bool validPlacement = false;

                // Check current tile is null
                AnimatedTile tile = tilemap.GetTile<AnimatedTile >(new Vector3Int(tilemapPos.x, tilemapPos.y, 0));
                if (tile == null) {

                    string[] neighbors = checkNeighbors(tilemapPos);
                    int numNeighbors = 0;

                    // Check for at least one neighbor
                    for (int i = 0; i < 6; i++) {
                        if (!neighbors[i].Equals("")) {
                            numNeighbors++;
                            if (numNeighbors > 1)
                            {
                                validPlacement = true;
                                break;
                            }
                            
                        }
                    }
                }

                // Set color overlay
                if (validPlacement) {
                    selectTilemap.color = validOverlay;
                } else {
                    selectTilemap.color = invalidOverlay;
                }

                //print(validPlacement);
            }
        }

        // Debug print grid position on right click
        /*if (Input.GetMouseButtonDown(1)) {
            tilemapPos = tilemap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            print(tilemapPos.x + ", " + tilemapPos.y);
        }*/

        // Cancel place tile
        if (Input.GetMouseButtonDown(1) && isPlacing) {
            isPlacing = false;
            circleMask.enabled = false;
        }

        // Place tile
        if (Input.GetMouseButtonDown(0) && isPlacing) {
            tilemapPos = tilemap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));

            // Check if current tile is occupied
            AnimatedTile tile = tilemap.GetTile<AnimatedTile >(new Vector3Int(tilemapPos.x, tilemapPos.y, 0));
            if (tile != null) {
                //print("fail!");
            } else {
                // Check if there is at least one neighbor

                string[] neighbors = checkNeighbors(tilemapPos);

                // Count neighbors
                int totalNeighbors = 0;
                for (int i = 0; i < 6; i++) {
                    if (!neighbors[i].Equals("")) {
                        totalNeighbors++;
                    }
                }
                //print(totalNeighbors);

                if (totalNeighbors > 0) {
                    // Special Cases - Advanced Tiles
                    // Forest: If placing woodland, check for 2 others
                    // Apiary: If placing beekeeper, check for 2 others
                    // Park: If placing park, meadow, or pond, check for other two
                    //print(selectedTile.name);

                    if (Input.GetKey(KeyCode.LeftShift)) {
                        // Attempt to repeat placement
                    } else {
                        isPlacing = false;
                        circleMask.enabled = false;
                    }
                    if (hv.BuyTile(selectedTile.name) == false)
                    {
                        return;
                    }
                    tilemap.SetTile(new Vector3Int(tilemapPos.x, tilemapPos.y, 0), selectedTile);
                    AudioController.instance.PlayOneShot(tileBasicSound, this.transform.position);

                    if (selectedTile.name == woodlandTileName) {
                        Debug.Log("WOODLAND UPGRADE");
                        replaceMatches(tilemapPos, forestTile);
                        AudioController.instance.SetParameter(music, "Forest", 1, this.transform.position);
                        AudioController.instance.PlayOneShot(tileAdvancedSound, this.transform.position);
                    } else if (selectedTile.name == beekeeperTileName) {
                        replaceMatches(tilemapPos, apiaryTile);
                        AudioController.instance.SetParameter(music, "Apiary", 1, this.transform.position);
                        AudioController.instance.PlayOneShot(tileAdvancedSound, this.transform.position);
                    } else if (selectedTile.name == gardenTileName || selectedTile.name == meadowTileName || selectedTile.name == pondTileName) {
                        // Park: garden + meadow + pond
                        matchingNeighbors = new List<Vector3Int>();
                        getMatchingNeighbors(tilemapPos, gardenTileName, meadowTileName, pondTileName);

                        // Check that there is at least one of each tile type
                        int gardenTiles = 0;
                        int meadowTiles = 0;
                        int pondTiles = 0;

                        List<string> neighborNames = getPositionNames(matchingNeighbors);
                        neighborNames.Add(selectedTile.name);

                        for (int i = 0; i < neighborNames.Count; i++) {
                            if (neighborNames[i] == gardenTileName) {
                                gardenTiles++;
                            } else if (neighborNames[i] == meadowTileName) {
                                meadowTiles++;
                            } else if (neighborNames[i] == pondTileName) {
                                pondTiles++;
                            }
                        }

                        if (gardenTiles >= 1 && meadowTiles >= 1 && pondTiles >= 1) {
                            // Convert all the matches
                            for (int i = 0; i < matchingNeighbors.Count; i++) {
                                tilemap.SetTile(new Vector3Int(matchingNeighbors[i].x, matchingNeighbors[i].y, 0), parkTile);
                            }
                            tilemap.SetTile(new Vector3Int(tilemapPos.x, tilemapPos.y, 0), parkTile);
                            AudioController.instance.SetParameter(music, "Park", 1, this.transform.position);
                            AudioController.instance.PlayOneShot(tileAdvancedSound, this.transform.position);
                        }
                        //print("gardens: " + gardenTiles + " | meadows: " + meadowTiles + " | ponds: " + pondTiles);
                        
                    }

                    pf.getPath();
                }
            }
        }
        circleMask.transform.position = new Vector3(mousePos.x, mousePos.y, 0);
    }

    List<string> getPositionNames(List<Vector3Int> arr) {
        List<string> names = new List<string>();
        for (int i = 0; i < arr.Count; i++) {
            AnimatedTile tile = tilemap.GetTile<AnimatedTile >(new Vector3Int(arr[i].x, arr[i].y, 0));
            if (tile != null) {
                names.Add(tile.name);
            } else {
                names.Add("");
            }
        }
        return names;
    }

    private void replaceMatches(Vector3Int tilemapPos, AnimatedTile replacementTile) {
        // Check for at least 2 others
        matchingNeighbors = new List<Vector3Int>();
        getMatchingNeighbors(tilemapPos, getTileType(selectedTile.name));

        print("found " + matchingNeighbors.Count + " matching neighbors.");

        if (matchingNeighbors.Count >= 3) {
            // Convert all the matches
            for (int i = 0; i < matchingNeighbors.Count; i++) {
                tilemap.SetTile(new Vector3Int(matchingNeighbors[i].x, matchingNeighbors[i].y, 0), replacementTile);
            }
            tilemap.SetTile(new Vector3Int(tilemapPos.x, tilemapPos.y, 0), replacementTile);
        }
    }

    public void Purchase(AnimatedTile tile) {
        //print(tile);
        selectedTile = tile;
        isPlacing = true;
        circleMask.enabled = true;
        selectTilemap.color = invalidOverlay;
    }

    List<Vector3Int> matchingNeighbors;

    // Populates a list of neighbors matching the given tile name
    void getMatchingNeighbors(Vector3Int position, string match1) {
        string[] neighborNames = checkNeighbors(position);
        Vector3Int[] neighborCoords = getNeighborCoords(position);

        // Check neighbors of the current tile
        for (int i = 0; i < 6; i++) {
            Debug.Log(neighborNames[i]);
            if (!neighborNames[i].Equals("") && getTileType(neighborNames[i]).Equals(match1) && !matchingNeighbors.Contains(neighborCoords[i])) {
                matchingNeighbors.Add(neighborCoords[i]);
                getMatchingNeighbors(neighborCoords[i], match1);
            }
        }
    }

    // Populates a list of neighbors matching any one of the given tile names
    void getMatchingNeighbors(Vector3Int position, string match1, string match2, string match3 /*int matches1, int matches2, int matches3*/) {
        string[] neighborNames = checkNeighbors(position);
        Vector3Int[] neighborCoords = getNeighborCoords(position);

        // Check neighbors of the current tile
        for (int i = 0; i < 6; i++) {
            if ((neighborNames[i].Equals(match1) || neighborNames[i].Equals(match2) ||
                 neighborNames[i].Equals(match3)) && !matchingNeighbors.Contains(neighborCoords[i])) {
                matchingNeighbors.Add(neighborCoords[i]);
                getMatchingNeighbors(neighborCoords[i], match1, match2, match3);
            }
        }
    }

    private string getTileType(string tileName)
    {
        string[] words = tileName.Split('_');
        Debug.Log(words.Length);
        return words[1];
    }
}
