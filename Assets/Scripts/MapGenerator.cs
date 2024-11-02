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
    public Tilemap map;
    public Tile borderTile, groundTile, chestTile;
    int chestsToPlace;
    int chestsPlaced = 0;
    int maxChests = 4;
    string path = Application.dataPath + "/TextMaps/map.txt";

    
  
    private void Start()
    {
        chestsToPlace = Random.Range(1, maxChests);
        string Textmap = GenerateMapString(23, 13);
        Debug.Log(Textmap);
        ConvertMapToTilemap(Textmap);
    }

    string GenerateMapString(int width, int height)
    {
        char [,] map = new char [width, height];

        // Define rules.
        char wall = '#';
        char door = '@';
        char chestTile = '$';
        char ground = '_';

        // Draw map using symbols.
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
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
                        map[x, y] = ground;
                    }
                }
                else
                {
                    // Draw the ground
                    map[x, y] = ground;
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
