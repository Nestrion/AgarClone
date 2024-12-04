using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class VirusSpawner : MonoBehaviour
{
    // Virus prefab to spawn
    /// <summary>
    /// The virus
    /// </summary>
    public GameObject Virus;

    // How many viruses we want to spawn
    /// <summary>
    /// The virus count
    /// </summary>
    public int virusCount = 5;

    // Spawn area boundaries
    /// <summary>
    /// The spawn area minimum
    /// </summary>
    public Vector2 spawnAreaMin = new Vector2(-28, -28);
    /// <summary>
    /// The spawn area maximum
    /// </summary>
    public Vector2 spawnAreaMax = new Vector2(28, 28);

    // List to track spawned virus objects
    /// <summary>
    /// The spawned viruses
    /// </summary>
    private List<GameObject> spawnedViruses;

    // Minimum distance between spawned viruses
    /// <summary>
    /// The minimum distance between viruses
    /// </summary>
    public float minimumDistanceBetweenViruses = 5f;

    /// <summary>
    /// Starts this instance.
    /// </summary>
    void Start()
    {
        // Initialize the list to hold spawned viruses
        spawnedViruses = new List<GameObject>();

        // Spawn viruses
        SpawnViruses(virusCount);
    }

    // Function to spawn multiple viruses
    /// <summary>
    /// Spawns the viruses.
    /// </summary>
    /// <param name="count">The count.</param>
    void SpawnViruses(int count)
    {
        int attempts = 0; // To prevent infinite loops
        int maxAttempts = count * 10; // Maximum attempts to find valid positions

        while (spawnedViruses.Count < count && attempts < maxAttempts)
        {
            Vector2 randomPosition = GetRandomPosition();

            if (IsValidSpawnPosition(randomPosition, spawnedViruses, minimumDistanceBetweenViruses))
            {
                GameObject virus = Instantiate(Virus, randomPosition, Quaternion.identity);
                spawnedViruses.Add(virus);
            }

            attempts++;
        }

        if (spawnedViruses.Count < count)
        {
            Debug.LogWarning("Not enough valid positions to spawn all viruses!");
        }
    }

    // Function to get a random position within the defined spawn area
    /// <summary>
    /// Gets the random position.
    /// </summary>
    /// <returns></returns>
    public Vector2 GetRandomPosition()
    {
        float x = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
        float y = Random.Range(spawnAreaMin.y, spawnAreaMax.y);
        return new Vector2(x, y);
    }

    // Function to check if the position is valid for spawning
    /// <summary>
    /// Determines whether [is valid spawn position] [the specified position].
    /// </summary>
    /// <param name="position">The position.</param>
    /// <param name="spawnedObjects">The spawned objects.</param>
    /// <param name="minimumDistance">The minimum distance.</param>
    /// <returns>
    ///   <c>true</c> if [is valid spawn position] [the specified position]; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsValidSpawnPosition(Vector2 position, List<GameObject> spawnedObjects, float minimumDistance)
    {
        foreach (var obj in spawnedObjects)
        {
            if (obj != null && Vector2.Distance(position, obj.transform.position) < minimumDistance)
            {
                return false;
            }
        }
        return true;
    }
}
