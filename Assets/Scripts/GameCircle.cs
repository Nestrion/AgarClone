using System;
using UnityEngine;

public class GameCircle : MonoBehaviour
{
    public float FirstRadius { get; private set; } = 0;
    public float Radius { get; private set; } = 0;

    public float GameCircleSizeScale()
    {
        return Radius / FirstRadius;
    }

    void Awake()
    {
        AssignWorldSpaceRadius();
        FirstRadius = Radius;
    }

    void AssignWorldSpaceRadius()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            Vector2 spriteSizeInPixels = spriteRenderer.sprite.rect.size;

            float pixelsPerUnit = spriteRenderer.sprite.pixelsPerUnit;

            Vector2 spriteSizeInWorldUnits = new Vector2(
                (spriteSizeInPixels.x / pixelsPerUnit) * transform.lossyScale.x,
                (spriteSizeInPixels.y / pixelsPerUnit) * transform.lossyScale.y
            );

            Radius = spriteSizeInWorldUnits.x / 2f;
        }
    }

    public void CombineCircles(GameCircle other)
    {
        Radius = Mathf.Sqrt(Radius * Radius + other.Radius * other.Radius);
    }

    public void SubtractCircle(GameCircle other)
    {
        Radius = Mathf.Sqrt(Radius * Radius - other.Radius * other.Radius);
    }

    public void PropagateFirstRadius(float firstRadius)
    {
        FirstRadius = firstRadius;
    }

    public void HalveCircle()
    {
        Radius = Mathf.Sqrt(Radius * Radius / 2);
    }
}