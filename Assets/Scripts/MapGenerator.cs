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
    public Tile borderTile, groundTile, chestTile, houseTile;

    // Variables.
    int chestsToPlace;
    int chestsPlaced = 0;
    int housesPlaced;
    int housesToPlace = 0;
    int xOffset;
    int yOffset;
    string path = Application.dataPath + "/TextMaps/map.txt";

    // Constants.
    int maxChests = 4;
    const int maxHouses = 4;


    private void Start()
    {
        // Define random chests and houses to place.
        chestsToPlace = Random.Range(1, maxChests);
        housesToPlace = Random.Range(1, maxHouses);

        // Define random x and y within a range.
        int randomX = Random.Range(5, 20);
        int randomY = Random.Range(5, 10);

        string Textmap = GenerateMapString(23, 13);

        ConvertMapToTilemap(Textmap);
    }

    string GenerateMapString(int width, int height)
    {
        char[,] map = new char[width, height];

        // Define rules.
        char wall = '#';
        char houseTile = '@';
        char chestTile = '$';
        char groundTile = '_';

        int randomX = Random.Range(3, width - 3);
        int randomY = Random.Range(3, height - 3);
        Debug.Log("Houses placed " + housesPlaced);
        // Draw map using symbols.
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                map[x, y] = groundTile;

                
                    if (x == randomX && y == randomY)
                    {
                        map[x, y] = houseTile;
                        housesPlaced++;
                    }
                

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
                        map.SetTile(tilePosition, houseTile);
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
