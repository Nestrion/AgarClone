using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class Food : MonoBehaviour
{
    private float foodMass = 0.5f;

    public float FoodMass
    {
        get { return foodMass; }
    }

    public void Relocate(Vector2 newPosition)
    {
        transform.position = newPosition;
    }
}
