using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class InfiniteTilemap : MonoBehaviour
{
    public Tilemap tilemap;
    public Camera mainCamera;
    public TileBase tile;

    private Vector3Int previousTilePosition;

    void Update()
    {
        // Get camera position in world space
        Vector3 cameraPosition = mainCamera.transform.position;

        // Convert camera position to tilemap grid position
        Vector3Int tilePosition = tilemap.WorldToCell(cameraPosition);

        // Check if the camera moved to a new tile
        if (tilePosition != previousTilePosition)
        {
            // Update the previous position
            previousTilePosition = tilePosition;

            // Call method to create new tiles around the current camera position
            GenerateInfiniteGrid(tilePosition);
        }
    }

    // Generate additional tiles around the current tile position
    void GenerateInfiniteGrid(Vector3Int centerTilePosition)
    {
        // Define how far around the camera to generate tiles
        int range = 10;

        for (int x = -range; x <= range; x++)
        {
            for (int y = -range; y <= range; y++)
            {
                Vector3Int newTilePosition = new Vector3Int(centerTilePosition.x + x, centerTilePosition.y + y, 0);
                if (!tilemap.HasTile(newTilePosition))
                {
                    tilemap.SetTile(newTilePosition, tile);
                }
            }
        }
    }
}