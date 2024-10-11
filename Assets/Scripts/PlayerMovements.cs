using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovements : MonoBehaviour
{
    public float maxSpeed = 5f;  // Maksymalna prędkość gracza
    public float minSpeed = 1f;  // Minimalna prędkość, aby uniknąć całkowitego zatrzymania
    public float accelerationFactor = 2f;  // Współczynnik kontroli przyspieszenia
    private Rigidbody2D rb;  // Referencja do komponentu Rigidbody2D

    void Start()
    {
        // Pobieramy komponent Rigidbody2D przypisany do gracza
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Pobieramy pozycję myszy w świecie gry
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Obliczamy dystans między graczem a pozycją myszy
        float distance = Vector2.Distance(rb.position, mousePosition);

        // Dostosowujemy prędkość na podstawie dystansu (przy użyciu wartości ograniczonej)
        float speed = Mathf.Lerp(minSpeed, maxSpeed, distance / accelerationFactor);
        Debug.Log("Speed: " + speed);

        // Obliczamy nową pozycję gracza przy użyciu fizyki
        Vector2 newPosition = Vector2.MoveTowards(rb.position, mousePosition, speed * Time.deltaTime);

        // Ruch przy użyciu Rigidbody2D
        rb.MovePosition(newPosition);
    }
}
