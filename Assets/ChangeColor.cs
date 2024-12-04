using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class ChangeColor : MonoBehaviour
{
    /// <summary>
    /// The sprite
    /// </summary>
    public SpriteRenderer sprite;

    /// <summary>
    /// Colors the changer.
    /// </summary>
    public void ColorChanger()
    {
        if (sprite == null)
        {
            Debug.LogError("SpriteRenderer is not assigned!");
            return;
        }
        sprite.color = new Color(
            UnityEngine.Random.Range(0f, 1f), // Losowy kolor dla czerwonego kanału (od 0 do 1)
            UnityEngine.Random.Range(0f, 1f), // Losowy kolor dla zielonego kanału (od 0 do 1)
            UnityEngine.Random.Range(0f, 1f)  // Losowy kolor dla niebieskiego kanału (od 0 do 1)
        );
    }
}