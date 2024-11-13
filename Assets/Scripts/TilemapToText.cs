using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapToText : MonoBehaviour
{
    public Tilemap emptyTilemap;

    string ConvertTiles(Tilemap[] tilemaps, string[] tilemapNames)
    {
        StringBuilder map = new StringBuilder();

        for (int i = 0; i < tilemaps.Length; i++)
        {
            // Add tilemap name at the start
            map.AppendLine(tilemapNames[i]);

            // Loop through the bounds of the tilemap
            BoundsInt bounds = tilemaps[i].cellBounds;
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                for (int x = bounds.xMin; x < bounds.xMax; x++)
                {
                    // Get the tile at the current position
                    TileBase tile = tilemaps[i].GetTile(new Vector3Int(x, y, 0));

                    // Add characters based on tile type
                    if (tile == null)
                    {
                        map.Append(' '); // Empty space
                    }
                    else if (tile.name == "GrassTile")
                    {
                        map.Append('_');
                    }
                    else if (tile.name == "ChestTile")
                    {
                        map.Append('$');
                    }
                    else if (tile.name == "HouseTile")
                    {
                        map.Append('@');
                    }
                    else if (tile.name == "PlayerTile")
                    {
                        map.Append('P');
                    }
                    else
                    {
                        map.Append('?'); // Unknown tile type
                    }
                }
                map.AppendLine(); // Newline after each row
            }

            map.AppendLine(); // Separate each tilemap with a newline
        }

        return map.ToString();
    }
}
