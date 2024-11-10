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

    private string[] mapPaths = {"Assets/textmaps/map.txt", "Assets/textmaps/map2.txt", 
        "Assets/textmaps/map3.txt", "Assets/textmaps/map4.txt", "Assets/textmaps/map5.txt"};
    
    private void Start()
    {
        chestsToPlace = Random.Range(1, maxChests);
        housesToPlace = Random.Range(1, maxHouses);

        string mapString = GenerateMapString(21, 13);
        TilemapConverter.ConvertMapToTilemap(mapString, map, playerLayer, borderTile, groundTile, chestTile, houseTiles, playerTile);
        PlayerController.SpawnPlayer(playerLayer, playerTile, map);
    }

    private string GenerateMapString(int width, int height)
    {
        char[,] map = new char[width, height];
        char wall = '#';
        char houseTile = '@';
        char chestTile = '$';
        char groundTile = '_';

        int chestsPlaced = 0;
        int housesPlaced = 0;
        int rollRange = 100;
        int houseChance = 50;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                map[x, y] = groundTile;

                if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
                {
                    map[x, y] = wall;
                }
                else if ((x == 1 && y == 1) || (x == width - 2 && y == 1) ||
                         (x == 1 && y == height - 2) || (x == width - 2 && y == height - 2))
                {
                    if (chestsPlaced < chestsToPlace)
                    {
                        map[x, y] = chestTile;
                        chestsPlaced++;
                    }
                }
                else if ((y > 3 && y < height - 3) && (x > 3 && x < width - 3))
                {
                    int randomRoll = Random.Range(0, rollRange);
                    if (housesPlaced < housesToPlace && randomRoll < houseChance)
                    {
                        map[x, y] = houseTile;
                        housesPlaced++;
                    }
                }
            }
        }

        System.Text.StringBuilder mapBuilder = new System.Text.StringBuilder();
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                mapBuilder.Append(map[x, y]);
            }
            mapBuilder.AppendLine();
        }
        return mapBuilder.ToString();
    }

    public void GenerateMap()
    {
        string randomMapPath = mapPaths[Random.Range(0, mapPaths.Length)];

        string mapData = MapLoader.LoadPremadeMap(randomMapPath);

        TilemapConverter.ConvertMapToTilemap(mapData, map, playerLayer, borderTile, groundTile,
            chestTile, houseTiles, playerTile);

        
    }
}