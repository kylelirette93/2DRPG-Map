using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Tilemaps;
public class MapGenerator : MonoBehaviour
{
    // References.
    public Tilemap map;
    public Tilemap playerLayer;
    public Tile borderTile, groundTile, chestTile, playerTile;
    public Tile[] houseTiles;
    

    // Variables.
    int chestsToPlace;
    int chestsPlaced = 0;
    int housesPlaced;
    int housesToPlace = 0;
    int xOffset;
    int yOffset;
    int playerPosX;
    int playerPosY;
    int playerPosition;
    public int rollRange;
    public int houseChance;
    int randomHouseIndex;
    Vector3Int currentPosition;
    string path = Application.dataPath + "/TextMaps/map.txt";

    // Constants.
    const int maxChests = 4;
    const int maxHouses = 4;


    private void Start()
    {
        // Define random chests and houses to place.
        chestsToPlace = Random.Range(1, maxChests);
        housesToPlace = Random.Range(1, 4);

        // Generate the map string.
        string Textmap = GenerateMapString(23, 13);

        // Convert string to tilemap.
        ConvertMapToTilemap(Textmap);

        // Spawn the player at initial position.
        SpawnPlayer();
    }

    private void Update()
    {
        // Gather input from the player.
        HandleInput();
    }

    string GenerateMapString(int width, int height)
    {
        char[,] map = new char[width, height];


        // Define rules.
        char wall = '#';
        char houseTile = '@';
        char chestTile = '$';
        char groundTile = '_';

        
        Debug.Log("Houses placed " + housesPlaced);
        // Draw map using symbols.
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                map[x, y] = groundTile;

                if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
                {
                    // Draw the border
                    map[x, y] = wall;
                }
                else if (x == 1 && y == 1 || x == width - 2 && y == 1 ||
                    x == 1 && y == height - 2 || x == width - 2 && y == height - 2)
                {
                    if (chestsPlaced <= chestsToPlace)
                    {
                        // Draw chests in the corner
                        map[x, y] = chestTile;
                        chestsPlaced++;
                    }
                    else
                    {
                        // Placed ground elsewhere
                        map[x, y] = groundTile;
                    }
                }
                else if ((y > 3 && y < height - 3) && (x > 3 && x < width - 3))
                {
                    // Do a random roll.
                    randomHouseIndex = Random.Range(0, houseTiles.Length);
                    int randomRoll = Random.Range(0, rollRange);

                    if (housesPlaced < housesToPlace && randomRoll < houseChance)
                    {
                        map[x, y] = houseTile;
                        housesPlaced++;
                    }
                    else
                    {
                        // If chests aren't to be drawn, draw ground instead
                        map[x, y] = groundTile;
                    }
                }
      
                else
                {
                    // Draw the ground
                    map[x, y] = groundTile;
                }
            }
        }

        // Convert character map to string using string builder.
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

    void SpawnPlayer()
    {
        playerPosX = map.cellBounds.xMin + map.cellBounds.size.x / 2;
        playerPosY = map.cellBounds.yMin + map.cellBounds.size.y / 2;
        currentPosition = new Vector3Int(playerPosX, playerPosY, 0);
        playerLayer.SetTile(currentPosition, playerTile);
        
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Debug.Log("Up arrow pressed");
            playerLayer.SetTile(currentPosition, null);
            Vector3Int up = new Vector3Int(0, 1, 0);
            currentPosition += up;
            playerLayer.SetTile(currentPosition, playerTile);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Debug.Log("Left arrow pressed");
            playerLayer.SetTile(currentPosition, null);
            Vector3Int left = new Vector3Int(-1, 0, 0);
            currentPosition += left;
            playerLayer.SetTile(currentPosition, playerTile);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Debug.Log("Right arrow pressed");
            playerLayer.SetTile(currentPosition, null);
            Vector3Int right = new Vector3Int(1, 0, 0);
            currentPosition += right;
            playerLayer.SetTile(currentPosition, playerTile);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Debug.Log("Left arrow pressed");
            playerLayer.SetTile(currentPosition, null);
            Vector3Int down = new Vector3Int(0, -1, 0);
            currentPosition += down;
            playerLayer.SetTile(currentPosition, playerTile);
        }
    }

    void ConvertMapToTilemap(string mapData)
    {
        // Split the string into new lines to represent rows.
        string[] rows = mapData.Split('\n');

        // Adjust tile placing based on Unity view.
        Vector3Int offset = new Vector3Int(-12, -7, 0);
        for (int y = 0; y < rows.Length; y++)
        {
            for (int x = 0; x < rows[y].Length; x++)
            {
                Vector3Int tilePosition = new Vector3Int(x, y, 0) + offset;
                switch (rows[y][x])
                {
                    case '#':
                        map.SetTile(tilePosition, borderTile);
                        break;
                    case '_':
                        map.SetTile(tilePosition, groundTile);
                        break;
                    case '$':
                        map.SetTile(tilePosition, chestTile);
                        break;
                    case '@':
                        randomHouseIndex = Random.Range(0, houseTiles.Length);
                        map.SetTile(tilePosition, houseTiles[randomHouseIndex]);
                        break;
                    case 'P':
                        map.SetTile(tilePosition, playerTile);
                        break;
                }
            }
        }
    }

    string LoadPremadeMap(string mapFilePath)
    {
        // Read text file from path and return it as a string.
        StreamReader streamReader = new StreamReader(mapFilePath);
        string content = streamReader.ReadToEnd();
        Debug.Log(content);
        return content;
    }
}