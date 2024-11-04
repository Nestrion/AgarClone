using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    // Food prefab to spawn
    public GameObject Food;

    // How many food items we want to spawn
    public int foodCount;

    // Spawn area boundaries (use a square or rectangular area)
    public Vector2 spawnAreaMin = new Vector2(-30, -30);
    public Vector2 spawnAreaMax = new Vector2(30, 30);

    // List to track spawned food objects (optional for managing them later)
    private List<GameObject> spawnedFood;

    Color[] colors = new Color[]
    {
        new Color(1f, 0f, 0f),     // Czerwony
        new Color(1f, 0.5f, 0f),   // Pomarañczowy
        new Color(1f, 1f, 0f),     // ¯ó³ty
        new Color(0f, 1f, 0f),     // Zielony
        new Color(0f, 0f, 1f),     // Niebieski
        new Color(0f, 0.5f, 1f),   // Jasnoniebieski
        new Color(0.5f, 0f, 0.5f), // Fioletowy
        new Color(0.5f, 0.5f, 0.5f), // Szary
        new Color(0.2f, 0.2f, 0.2f), // Ciemnoszary
        new Color(0f, 0f, 0f),     // Czarny
    };

    void Start()
    {
        // Initialize the list to hold spawned food
        spawnedFood = new List<GameObject>();

        // Spawn initial food items
        SpawnFood(foodCount);
    }

    // Function to spawn multiple food items
    void SpawnFood(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Vector2 randomPosition = GetRandomPosition();
            GameObject food = Instantiate(Food, randomPosition, Quaternion.identity);

            SpriteRenderer renderer = food.GetComponent<SpriteRenderer>();
            Color randomColor = colors[Random.Range(0, colors.Length)];
            renderer.material.color = randomColor;

            spawnedFood.Add(food);
        }
    }

    // Function to get a random position within the defined spawn area
    Vector2 GetRandomPosition()
    {
        float x = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
        float y = Random.Range(spawnAreaMin.y, spawnAreaMax.y);
        return new Vector2(x, y);
    }

    // Optionally, call this function to spawn more food later
    public void SpawnAdditionalFood(int additionalCount)
    {
        SpawnFood(additionalCount);
    }

    // (Optional) You can add a method to remove or replace food
    public void RemoveFood(GameObject food)
    {
        spawnedFood.Remove(food);
        Destroy(food);
    }
}