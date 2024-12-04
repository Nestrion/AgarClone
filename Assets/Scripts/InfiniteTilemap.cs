using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// 
/// </summary>
public class InfiniteTilemap : MonoBehaviour
{
    /// <summary>
    /// The tilemap
    /// </summary>
    public Tilemap tilemap;
    /// <summary>
    /// The main camera
    /// </summary>
    public Camera mainCamera;
    /// <summary>
    /// The tile
    /// </summary>
    public TileBase tile;

    /// <summary>
    /// The previous tile position
    /// </summary>
    private Vector3Int previousTilePosition;

    /// <summary>
    /// Starts this instance.
    /// </summary>
    void Start()
    {
        InitialGeneration();
    }

    /// <summary>
    /// Updates this instance.
    /// </summary>
    void Update()
    {
        GenerateInfiniteGridOnCamereMovement();
    }

    /// <summary>
    /// Initials the generation.
    /// </summary>
    void InitialGeneration()
    {
        Vector3 cameraPosition = mainCamera.transform.position;
        Vector3Int tilePosition = tilemap.WorldToCell(cameraPosition);
        GenerateInfiniteGrid(tilePosition);
    }

    /// <summary>
    /// Generates the infinite grid on camere movement.
    /// </summary>
    void GenerateInfiniteGridOnCamereMovement()
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
    /// <summary>
    /// Generates the infinite grid.
    /// </summary>
    /// <param name="centerTilePosition">The center tile position.</param>
    void GenerateInfiniteGrid(Vector3Int centerTilePosition)
    {
        // Define how far around the camera to generate tiles
        int range = 20;

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