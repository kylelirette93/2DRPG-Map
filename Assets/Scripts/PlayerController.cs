using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    private static Vector3Int currentPosition;
    private static Vector3Int initialPosition;
    private static Tilemap playerLayer;
    private static Tilemap map;
    private static Tile playerTile;

    public static void SpawnPlayer(Tilemap playerLayer, Tile playerTile, Tilemap map)
    {
        // Assign layer, tile and map to static variables to maintain state of player.
        PlayerController.playerLayer = playerLayer;
        PlayerController.playerTile = playerTile;
        PlayerController.map = map;

        // Player's X and Y is in the middle of map.
        int playerPosX = map.cellBounds.xMin + map.cellBounds.size.x / 2;
        int playerPosY = map.cellBounds.yMin + map.cellBounds.size.y / 2;

        // Assign player's current position to the middle.
        currentPosition = new Vector3Int(playerPosX, playerPosY, 0);

        // Store the initial position of the player.
        initialPosition = currentPosition;

        // Set the tile with initial position on the map.
        playerLayer.SetTile(initialPosition, playerTile);
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

    public static void ResetPlayerPosition()
    {
        // Resets player's position to initial position.
        playerLayer.SetTile(currentPosition, null);
        currentPosition = initialPosition;
        playerLayer.SetTile(currentPosition, playerTile);
    }

    

    private void MovePlayer(Vector3Int direction)
    {
        // Add direction to current position.
        Vector3Int newPosition = currentPosition + direction;
        if (CanMoveTo(newPosition))
        {
            // Set player's current position to new position.
            playerLayer.SetTile(currentPosition, null);
            currentPosition = newPosition;
            playerLayer.SetTile(currentPosition, playerTile);
        }
    }

    private bool CanMoveTo(Vector3Int position)
    {
        // Takes where player moves to as parameter.

        if (map == null || playerLayer == null)
        {
            Debug.LogError("Map or playerLayer is not assigned.");
            return false;
        }

        // Get tile as position on the map.
        TileBase mapTile = map.GetTile(position);
        TileBase playerLayerTile = playerLayer.GetTile(position);

        // Check if the position is empty on both layers.
        if (mapTile == null && playerLayerTile == null)
        {
            // If position is empty player can move.
            return true; 
        }

        // Apply rules when comparing player's new position.
        // Player cannot walk through walls, chests or houses.
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
        // If no conditions are met, player can move.
        return true;
    }
}