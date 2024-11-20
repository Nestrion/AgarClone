using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    // Food prefab to spawn
    public GameObject Food;

    // How many food items we want to spawn
    public int foodCount;
    public int maxFoodCount;

    // Spawn area boundaries (use a square or rectangular area)
    Vector2 spawnAreaMin = new Vector2(-28, -28);
    Vector2 spawnAreaMax = new Vector2(28, 28);

    // List to track spawned food objects (optional for managing them later)
    public List<GameObject> spawnedFood;

    Color[] colors = new Color[]
    {
        new Color(1f, 0f, 0f),     // Czerwony
        new Color(1f, 0.5f, 0f),   // Pomara�czowy
        new Color(1f, 1f, 0f),     // ��ty
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

            int maxAttempts = 10; // Prevent infinite loops
            int attempts = 0;
            
            while (!IsValidSpawnPosition(randomPosition, spawnedFood) && attempts < maxAttempts)
            {
                randomPosition = new Vector3(
                    Random.Range(spawnAreaMin.x, spawnAreaMax.x),
                    Random.Range(spawnAreaMin.y, spawnAreaMax.y),
                    0
                );
                attempts++;
            }

            if (attempts < maxAttempts)
            {
            GameObject food = Instantiate(Food, randomPosition, Quaternion.identity);
            SpriteRenderer renderer = food.GetComponent<SpriteRenderer>();
            Color randomColor = colors[Random.Range(0, colors.Length)];
            renderer.material.color = randomColor;

            spawnedFood.Add(food);
            }
        }
    }

    /*void RespawnFood(){

        Vector2 randomPosition = GetRandomPosition();

        int maxAttempts = 10; // Prevent infinite loops
        int attempts = 0;
        
        while (!IsValidSpawnPosition(randomPosition, spawnedFood) && attempts < maxAttempts)
        {
            randomPosition = new Vector3(
                Random.Range(spawnAreaMin.x, spawnAreaMax.x),
                Random.Range(spawnAreaMin.y, spawnAreaMax.y),
                0
            );
            attempts++;
        }

        if (attempts < maxAttempts)
        {
        GameObject food = Instantiate(Food, randomPosition, Quaternion.identity);
        SpriteRenderer renderer = food.GetComponent<SpriteRenderer>();
        Color randomColor = colors[Random.Range(0, colors.Length)];
        renderer.material.color = randomColor;
        spawnedFood.Add(food);
        }
    }
*/
    // Function to get a random position within the defined spawn area
    Vector2 GetRandomPosition()
    {
        float x = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
        float y = Random.Range(spawnAreaMin.y, spawnAreaMax.y);
        return new Vector2(x, y);
    }

    public static bool IsValidSpawnPosition(Vector2 position, List<GameObject> spawnedFood, float minimumDistance = 2f)
    {
        foreach (var food in spawnedFood)
    {
        if ((position - (Vector2)food.transform.position).sqrMagnitude < minimumDistance * minimumDistance)
        {
            return false;
        }
    }
    return true;
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

    /*void Update(){

        if (spawnedFood.Count <= maxFoodCount) {
            //RespawnFood();
        }

        //spawnedFood.RemoveAll(obj => obj == null);

        // PRZEROBIC NA PRZENIESIENIE JEDZENIA A NIE NISZCZENIE

        Debug.Log(spawnedFood.Count);
    }*/
}