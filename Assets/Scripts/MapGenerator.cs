using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    public Tilemap map;
    public Tilemap playerLayer;
    public Tile borderTile, groundTile, chestTile, playerTile;
    public Tile[] houseTiles;

    private int chestsToPlace;
    private int housesToPlace;
    private const int maxChests = 4;
    private const int maxHouses = 4;

    private string[] mapPaths = {"map.txt", "map2.txt", 
        "map3.txt", "map4.txt", "map5.txt"};
    
    private void Start()
    {
        // Random number of chest and houses to place.
        chestsToPlace = Random.Range(1, maxChests);
        housesToPlace = Random.Range(1, maxHouses);

        // Generate a default text map based on size and rules.
        string mapString = GenerateMapString(21, 13);

        // Convert map to tilemap.
        TilemapConverter.ConvertMapToTilemap(mapString, map, playerLayer, borderTile, groundTile, chestTile, houseTiles, playerTile);
        
        // Spawn player on the map.
        PlayerController.SpawnPlayer(playerLayer, playerTile, map);
    }

    private string GenerateMapString(int width, int height)
    {
        // Array to hold map characters.
        char[,] map = new char[width, height];
        char wall = '#';
        char houseTile = '@';
        char chestTile = '$';
        char groundTile = '_';

        int chestsPlaced = 0;
        int housesPlaced = 0;

        // These variables are used to determine the chance of placing a house.
        int rollRange = 100;
        int houseChance = 50;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                map[x, y] = groundTile;

                // The border/wall of the map filled with trees.
                if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
                {
                    map[x, y] = wall;
                }
                // The corners of the map where chests are placed.
                else if ((x == 1 && y == 1) || (x == width - 2 && y == 1) ||
                         (x == 1 && y == height - 2) || (x == width - 2 && y == height - 2))
                {
                    if (chestsPlaced < chestsToPlace)
                    {
                        map[x, y] = chestTile;
                        chestsPlaced++;
                    }
                }
                // Creates a box inside the map where houses can be placed.
                else if ((y > 3 && y < height - 3) && (x > 3 && x < width - 3))
                {
                    // Determine a random roll between 0 and 100.
                    int randomRoll = Random.Range(0, rollRange);

                    // Player has 50% chance of placing a house, as long as less than max houses are placed.
                    if (housesPlaced < housesToPlace && randomRoll < houseChance)
                    {
                        map[x, y] = houseTile;
                        housesPlaced++;
                    }
                }
            }
        }

        // Create an instance of string builder.
        System.Text.StringBuilder mapBuilder = new System.Text.StringBuilder();
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                // Append current map character to the string builder.
                mapBuilder.Append(map[x, y]);
            }
            // Adds a new line after each row.
            mapBuilder.AppendLine();
        }
        // Return converted string.
        return mapBuilder.ToString();
    }

    public void GenerateMap()
    {
        // Reset player's position when generating new map.
        PlayerController controller = FindObjectOfType<PlayerController>();
        PlayerController.ResetPlayerPosition();

        // Select a random map from the list of map paths.
        string randomMapPath = mapPaths[Random.Range(0, mapPaths.Length)];

        // Load and convert the map data to a tilemap.
        string mapData = MapLoader.LoadPremadeMap(randomMapPath);
        TilemapConverter.ConvertMapToTilemap(mapData, map, playerLayer, borderTile, groundTile,
            chestTile, houseTiles, playerTile);        
    }
}