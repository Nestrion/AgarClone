using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class FoodSpawner : MonoBehaviour
{
    // Food prefab to spawn
    /// <summary>
    /// The food
    /// </summary>
    public GameObject Food;

    // How many food items we want to spawn
    /// <summary>
    /// The food count
    /// </summary>
    public int foodCount;

    // Spawn area boundaries (use a square or rectangular area)
    /// <summary>
    /// The spawn area minimum
    /// </summary>
    public Vector2 spawnAreaMin = new Vector2(-28, -28);
    /// <summary>
    /// The spawn area maximum
    /// </summary>
    public Vector2 spawnAreaMax = new Vector2(28, 28);

    // List to track spawned food objects (optional for managing them later)
    /// <summary>
    /// The spawned food
    /// </summary>
    public List<GameObject> spawnedFood;

    /// <summary>
    /// The colors
    /// </summary>
    Color[] colors = new Color[]
    {
        new Color(1f, 0f, 0f),     // Czerwony
        new Color(1f, 0.5f, 0f),   // Pomara�czowy
        new Color(1f, 1f, 0f),     // ��ty
        new Color(0f, 0f, 1f),     // Niebieski
        new Color(0f, 0.5f, 1f),   // Jasnoniebieski
        new Color(0.5f, 0f, 0.5f), // Fioletowy
        new Color(0.5f, 0.5f, 0.5f), // Szary
        new Color(0.2f, 0.2f, 0.2f), // Ciemnoszary
        new Color(0f, 0f, 0f),     // Czarny
    };

    /// <summary>
    /// Starts this instance.
    /// </summary>
    void Start()
    {
        // Initialize the list to hold spawned food
        spawnedFood = new List<GameObject>();

        // Spawn initial food items
        SpawnFood(foodCount);
    }

    // Function to spawn multiple food items
    /// <summary>
    /// Spawns the food.
    /// </summary>
    /// <param name="count">The count.</param>
    void SpawnFood(int count)
    {
        for (int i = 0; spawnedFood.Count <= 200; i++)
        {
            Vector2 randomPosition = GetRandomPosition();

            if (IsValidSpawnPosition(randomPosition, spawnedFood)){
                
                GameObject food = Instantiate(Food, randomPosition, Quaternion.identity);

                float minScale = 0.40f;
                float maxScale = 0.80f;
                float randomScale = Random.Range(minScale, maxScale);
                food.transform.localScale = new Vector3(randomScale, randomScale, randomScale);
                Food foodComponent = food.GetComponent<Food>();
                foodComponent.FoodMass = randomScale * 2;
                SpriteRenderer renderer = food.GetComponent<SpriteRenderer>();
                Color randomColor = colors[Random.Range(0, colors.Length)];
                renderer.material.color = randomColor;

            spawnedFood.Add(food);
            }
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


    /// <summary>
    /// Determines whether [is valid spawn position] [the specified position].
    /// </summary>
    /// <param name="position">The position.</param>
    /// <param name="spawnedFood">The spawned food.</param>
    /// <param name="minimumDistance">The minimum distance.</param>
    /// <returns>
    ///   <c>true</c> if [is valid spawn position] [the specified position]; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsValidSpawnPosition(Vector2 position, List<GameObject> spawnedFood, float minimumDistance = 2f)
{
    foreach (var food in spawnedFood)
    {
        if (food != null && Vector2.Distance(position, food.transform.position) < minimumDistance)
        {
            return false;
        }
    }
    return true;
}
}