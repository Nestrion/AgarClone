using System.Collections.Generic;
using UnityEngine;

public class VirusSpawner : MonoBehaviour
{
    // Virus prefab to spawn
    public GameObject Virus;

    // How many viruses we want to spawn
    public int virusCount = 5;

    // Spawn area boundaries
    public Vector2 spawnAreaMin = new Vector2(-28, -28);
    public Vector2 spawnAreaMax = new Vector2(28, 28);

    // List to track spawned virus objects
    private List<GameObject> spawnedViruses;

    // Minimum distance between spawned viruses
    public float minimumDistanceBetweenViruses = 5f;

    void Start()
    {
        // Initialize the list to hold spawned viruses
        spawnedViruses = new List<GameObject>();

        // Spawn viruses
        SpawnViruses(virusCount);
    }

    // Function to spawn multiple viruses
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
    public Vector2 GetRandomPosition()
    {
        float x = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
        float y = Random.Range(spawnAreaMin.y, spawnAreaMax.y);
        return new Vector2(x, y);
    }

    // Function to check if the position is valid for spawning
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
