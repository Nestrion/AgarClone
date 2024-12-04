using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class FoodConsumer : MonoBehaviour
{
    /// <summary>
    /// The audio manager
    /// </summary>
    private AudioManager audioManager;

    /// <summary>
    /// The circle collider2 d
    /// </summary>
    CircleCollider2D circleCollider2D;

    // will refactor
    /// <summary>
    /// The player
    /// </summary>
    Player player;
    /// <summary>
    /// The food spawner
    /// </summary>
    public FoodSpawner foodSpawner;
    /// <summary>
    /// The enemy
    /// </summary>
    public EnemyAI enemy;

    /// <summary>
    /// The main camera
    /// </summary>
    Camera mainCamera;

    /// <summary>
    /// The target orthographic size
    /// </summary>
    private float targetOrthographicSize;
    /// <summary>
    /// The lerp speed
    /// </summary>
    private float lerpSpeed = 2f;

    /// <summary>
    /// Starts this instance.
    /// </summary>
    void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        circleCollider2D = GetComponent<CircleCollider2D>();
        player = GetComponent<Player>();
        mainCamera = Camera.main;
        targetOrthographicSize = mainCamera.orthographicSize;
    }

    /// <summary>
    /// Called when [trigger stay2 d].
    /// </summary>
    /// <param name="other">The other.</param>
    private void OnTriggerStay2D(Collider2D other)
    {
        ConsumeFood(other);
    }

    /// <summary>
    /// Updates this instance.
    /// </summary>
    private void Update()
    {
        mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, targetOrthographicSize, Time.deltaTime * lerpSpeed);
    }

    /// <summary>
    /// Consumes the food.
    /// </summary>
    /// <param name="other">The other.</param>
    private void ConsumeFood(Collider2D other)
    {
        float distanceBetweenCenters = Vector2.Distance(transform.position, other.transform.position);

        Food food = other.gameObject.GetComponent<Food>();
        if (distanceBetweenCenters <= circleCollider2D.radius * transform.lossyScale.x)
            {
                //Food food = other.gameObject.GetComponent<Food>();
                if (food != null)
                {
                    // Get a new valid position for the food
                    Vector2 newPosition = foodSpawner.GetRandomPosition();
                    food.Relocate(newPosition);

                    // Odtwórz dźwięk jedzenia
                    audioManager.Play("FoodSound");

                    // Update player properties
                    player.PlayerScore += 1;
                    player.PlayerMass += food.FoodMass;
                    targetOrthographicSize += player.PlayerMass * 0.001f;
                }
            }
    }

}
