using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Aseprite;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class MapEditor : MonoBehaviour
{
    public Tilemap tilemap;

    public Tile GrassTile;
    public Tile ChestTile;
    public Tile TreeTile;
    public Tile TreeTile2;
    public Tile HouseTile;

    Tile selectedTile;

    public void ClickTileButton(string buttonName)
    {
        switch (buttonName)
        {
            case "GrassButton":
                selectedTile = GrassTile;
                break;
                case "ChestButton":
                selectedTile = ChestTile;
                break;
                case "TreeButton":
                selectedTile = TreeTile;
                break;
                case "TreeButton2":
                selectedTile = TreeTile2;
                break;
                case "HouseButton":
                selectedTile = HouseTile;
                break;
            case "EraserButton":
                selectedTile = null;
                break;

            default:
                selectedTile = null;
                break;
        }
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && !IsPointerOverUI())
        {
            Vector3Int cellPosition = GetMouseCellPosition();

            if (cellPosition != null)
            {
                tilemap.SetTile(cellPosition, selectedTile);
            }
        }
    }

    private bool IsPointerOverUI()
    {
        return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
    }

    Vector3Int GetMouseCellPosition()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0;

        Vector3Int cellPosition = tilemap.WorldToCell(mouseWorldPos);

        return cellPosition;
    }
    
}
