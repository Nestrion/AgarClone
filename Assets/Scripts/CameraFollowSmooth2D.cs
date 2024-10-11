using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowSmooth2D : MonoBehaviour
{
    public Transform player;  // Referencja do gracza
    public float smoothSpeed = 0.025f;  // Im mniejsza wartość, tym wolniejsza kamera
    public Vector3 offset;    // Opcjonalny offset (odstęp) od gracza

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