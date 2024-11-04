using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FoodConsumer : MonoBehaviour
{
    CircleCollider2D circleCollider2D;

    // will refactor
    Player player;

    Camera mainCamera;

    private float targetOrthographicSize;
    private float lerpSpeed = 2f;

    void Start()
    {
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

        if (distanceBetweenCenters <= circleCollider2D.radius * transform.lossyScale.x)
        {
            Food food = other.gameObject.GetComponent<Food>();
            if (food == true)
            {
                food.Consume();

                // will refactor
                player.PlayerScore += 1;

                player.PlayerMass += food.FoodMass;
                targetOrthographicSize += player.PlayerMass*0.001f;
            }
        }
    }

}
