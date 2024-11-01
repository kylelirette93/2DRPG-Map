using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Tilemaps;
public class MapGenerator : MonoBehaviour
{
    public Tilemap map;
    public Tile borderTile, groundTile;

    
  
    private void Start()
    {
        string Textmap = GenerateMapString(25, 14);
        Debug.Log(Textmap);
        ConvertMapToTilemap(Textmap);
    }

    string GenerateMapString(int width, int height)
    {
        char [,] map = new char [width, height];
        // Define rules.
        char wall = '#';
        char door = '@';
        char chest = '$';
        char floor = '_';

        // Draw map using symbols.
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
                {
                    map[x, y] = wall;
                }
                else
                {
                    map[x, y] = floor;
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
        Vector3Int offset = new Vector3Int(-12, -6, 0);
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
                }
            }
        }
    }
}
