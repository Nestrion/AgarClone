using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodConsumer : MonoBehaviour
{
    CircleCollider2D circleCollider2D;

    void Start()
    {
        circleCollider2D = GetComponent<CircleCollider2D>();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        ConsumeFood(other);
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
            }
        }

    }

}
