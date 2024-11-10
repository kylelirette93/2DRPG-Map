using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    private static Vector3Int currentPosition;
    private static Tilemap playerLayer;
    private static Tilemap map;
    private static Tile playerTile;

    public static void SpawnPlayer(Tilemap playerLayer, Tile playerTile, Tilemap map)
    {
        PlayerController.playerLayer = playerLayer;
        PlayerController.playerTile = playerTile;
        PlayerController.map = map;

        int playerPosX = map.cellBounds.xMin + map.cellBounds.size.x / 2;
        int playerPosY = map.cellBounds.yMin + map.cellBounds.size.y / 2;
        currentPosition = new Vector3Int(playerPosX, playerPosY, 0);
        playerLayer.SetTile(currentPosition, playerTile);
    }

    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            MovePlayer(new Vector3Int(0, 1, 0));
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MovePlayer(new Vector3Int(-1, 0, 0));
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MovePlayer(new Vector3Int(1, 0, 0));
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            MovePlayer(new Vector3Int(0, -1, 0));
        }
    }

    

    private void MovePlayer(Vector3Int direction)
    {
        Vector3Int newPosition = currentPosition + direction;
        if (CanMoveTo(newPosition))
        {
            playerLayer.SetTile(currentPosition, null);
            currentPosition = newPosition;
            playerLayer.SetTile(currentPosition, playerTile);
        }
    }

    private bool CanMoveTo(Vector3Int position)
    {
        if (map == null || playerLayer == null)
        {
            Debug.LogError("Map or playerLayer is not assigned.");
            return false;
        }

        TileBase mapTile = map.GetTile(position);
        TileBase playerLayerTile = playerLayer.GetTile(position);

        // Check if the position is empty on both layers
        if (mapTile == null && playerLayerTile == null)
        {
            return true; 
        }

        // Add logic to check for specific tile types
        if (mapTile == map.GetComponent<MapGenerator>().borderTile)
        {
            return false; 
        }
        if (mapTile == map.GetComponent<MapGenerator>().chestTile)
        {
            return false;
        }
        if (mapTile.name == "HouseTile")
        {
            return false;
        }

        return true;
    }
}