using UnityEngine;

public class Virus : MonoBehaviour
{
    public GameObject smallCirclePrefab; // Prefab mniejszych kół
    public int numberOfSmallCircles = 5; // Liczba małych kół po rozpadzie
    public float explosionForce = 5f; // Siła rozrzutu małych kół
    public float pullForce = 1f; // Siła przyciągania do oryginalnego gracza
    public float baseStopDistance = 1.5f; // Odległość zatrzymania przyciągania
    public float massStopScaleFactor = 0.1f; // Skala odległości zatrzymania na podstawie masy

    

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Sprawdź, czy obiekt, który dotknął wirusa, to gracz
        if (collision.gameObject.CompareTag("Player"))
        {
            // Zniszcz oryginalny obiekt gracza
            GameObject player = collision.gameObject;
            Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();

            if (playerRb != null)
            {
                Vector3 explosionCenter = player.transform.position;

                // Rozdzielenie na mniejsze koła
                for (int i = 0; i < numberOfSmallCircles; i++)
                {
                    // Stwórz nowe małe kółka
                    GameObject smallCircle = Instantiate(smallCirclePrefab, explosionCenter, Quaternion.identity);

                    // Ustaw ich masę
                    Rigidbody2D rb = smallCircle.GetComponent<Rigidbody2D>();
                    if (rb != null)
                    {
                        rb.mass = 3f;

                        // Dodaj siłę odpychającą
                        Vector2 explosionDirection = (Vector2)(Quaternion.Euler(0, 0, (360f / numberOfSmallCircles) * i) * Vector2.right);
                        rb.AddForce(explosionDirection * explosionForce, ForceMode2D.Impulse);

                        // Dodaj skrypt do przyciągania do oryginalnej pozycji
                        SplitPlayer splitPlayer = smallCircle.AddComponent<SplitPlayer>();
                        splitPlayer.originalPlayer = player.GetComponent<Player>();
                        splitPlayer.pullForce = pullForce;
                        splitPlayer.baseStopDistance = baseStopDistance;
                        splitPlayer.massStopScaleFactor = massStopScaleFactor;
                    }
                }

                // Zniszcz oryginalny obiekt gracza
                Destroy(player);
            
            }
        }
    }
}
