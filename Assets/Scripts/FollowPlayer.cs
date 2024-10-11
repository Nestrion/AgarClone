using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowSmooth : MonoBehaviour
{
    public Transform player;  // Referencja do gracza
    public float smoothSpeed = 0.125f;  // Im mniejsza wartość, tym wolniejsza kamera
    public Vector3 offset;    // Opcjonalny offset (odstęp) od gracza

    void FixedUpdate()
    {
        // Pozycja docelowa kamery (pozycja gracza z offsetem)
        Vector3 desiredPosition = player.position + offset;

        // Płynne przejście między aktualną pozycją kamery a docelową
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Ustawiamy nową pozycję kamery
        transform.position = smoothedPosition;
    }
}