using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class CameraFollowSmooth2D : MonoBehaviour
{
    /// <summary>
    /// The player
    /// </summary>
    public Transform player;  // Referencja do gracza
    /// <summary>
    /// The smooth speed
    /// </summary>
    public float smoothSpeed = 0.025f;  // Im mniejsza wartość, tym wolniejsza kamera
    /// <summary>
    /// The offset
    /// </summary>
    public Vector3 offset;    // Opcjonalny offset (odstęp) od gracza

    /// <summary>
    /// Fixeds the update.
    /// </summary>
    void FixedUpdate()
    {
        // Pozycja docelowa kamery (z zachowaniem pozycji Z kamery)
        Vector3 desiredPosition = new Vector3(player.position.x + offset.x, player.position.y + offset.y, transform.position.z);

        // Płynne przejście między aktualną pozycją kamery a docelową
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Ustawiamy nową pozycję kamery
        transform.position = smoothedPosition;
       
    }

}