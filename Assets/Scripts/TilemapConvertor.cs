using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapConverter
{
    public static void ConvertMapToTilemap(string mapData, Tilemap map, Tilemap playerLayer, Tile borderTile, Tile groundTile, Tile chestTile, Tile[] houseTiles, Tile playerTile)
    {
        string[] rows = mapData.Split('\n');
        Vector3Int offset = new Vector3Int(-14, -7, 0);

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
                        int randomHouseIndex = Random.Range(0, houseTiles.Length);
                        Tile houseTile = houseTiles[randomHouseIndex];
                        houseTile.name = "HouseTile"; 
                        map.SetTile(tilePosition, houseTile);
                        break;
                    case 'P':
                        playerLayer.SetTile(tilePosition, playerTile);
                        break;
                }
            }
        }
    }
}