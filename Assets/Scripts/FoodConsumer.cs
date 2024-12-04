using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FoodConsumer : MonoBehaviour
{
    private AudioManager audioManager;

    CircleCollider2D circleCollider2D;

    // will refactor
    Player player;
    public FoodSpawner foodSpawner;
    public EnemyAI enemy;

    Camera mainCamera;

    private float targetOrthographicSize;
    private float lerpSpeed = 2f;

    void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        circleCollider2D = GetComponent<CircleCollider2D>();
        player = GetComponent<Player>();
        mainCamera = Camera.main;
        targetOrthographicSize = mainCamera.orthographicSize;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        ConsumeFood(other);
    }

    private void Update()
    {
        mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, targetOrthographicSize, Time.deltaTime * lerpSpeed);
    }

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
