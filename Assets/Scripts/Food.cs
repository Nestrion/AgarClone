using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class Food : MonoBehaviour
{
    /// <summary>
    /// The food mass
    /// </summary>
    private float foodMass;

    /// <summary>
    /// Gets the food mass.
    /// </summary>
    /// <value>
    /// The food mass.
    /// </value>
    public float FoodMass
    {
        get { return foodMass; }
        set { foodMass = value; }
    }

    /// <summary>
    /// Sets the food mass.
    /// </summary>
    /// <param name="foodMassValue">The food mass value.</param>
    public void SetFoodMass(float foodMassValue)
    {
        foodMass = foodMassValue;
    }

    /// <summary>
    /// Relocates the specified new position.
    /// </summary>
    /// <param name="newPosition">The new position.</param>
    public void Relocate(Vector2 newPosition)
    {
        transform.position = newPosition;
    }
}
