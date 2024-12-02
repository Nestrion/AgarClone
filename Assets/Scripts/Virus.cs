using UnityEngine;
using System.Collections;

public class Virus : MonoBehaviour
{
    public GameObject smallCirclePrefab; // Prefab mniejszych kół
    public float explosionForce = 5f; // Siła rozrzutu małych kół
    public float chunkSize = 10f; // Bazowa masa kulki
    public float pullForce = 1f; // Siła przyciągania do oryginalnego gracza
    public float baseStopDistance = 1.5f; // Odległość zatrzymania przyciągania
    public float massStopScaleFactor = 0.1f; // Skala odległości zatrzymania na podstawie masy

    private AudioManager audioManager;

    private void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.GetComponent<CircleCollider2D>().radius * collision.transform.lossyScale.x);
        Debug.Log(gameObject.GetComponent<CircleCollider2D>().radius * transform.lossyScale.x);
        // Odtwórz dźwięk zetknięcia z wirusem
        audioManager.Play("HitVirus");

        // Sprawdź, czy obiekt, który dotknął wirusa, to gracz
        if (collision.gameObject.CompareTag("Player") &&
            collision.gameObject.GetComponent<CircleCollider2D>().radius * collision.transform.lossyScale.x >
            gameObject.GetComponent<CircleCollider2D>().radius * transform.lossyScale.x)
        {
            

            GameObject player = collision.gameObject;
            Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();

            if (playerRb != null)
            {
                Vector3 explosionCenter = player.transform.position;

                float playerMass = collision.gameObject.GetComponent<Player>().PlayerMass; // Liczba małych kół po rozpadzie
                float numberOfSmallCircles = (playerMass / chunkSize) - 1;
                float lastChunkAdditionalSize = playerMass % chunkSize;

                // Odtwórz dźwięk podziału gracza
                audioManager.Play("SplitVirus");

                // Rozdzielenie na mniejsze koła
                for (int i = 0; i < numberOfSmallCircles; i++)
                {
                    // Stwórz nowe małe kółka
                    GameObject smallCircle = Instantiate(smallCirclePrefab, explosionCenter, Quaternion.identity);
                    smallCircle.tag = "playerSplitted";

                    smallCircle.GetComponent<Player>().PlayerMass = chunkSize;

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

                // Ustaw resztkową masę gracza (co zostało z dzielenia)
                collision.gameObject.GetComponent<Player>().PlayerMass = lastChunkAdditionalSize;

                // Rozpocznij odliczanie do scalania
                StartCoroutine(MergeBallsAfterDelay(20f));
            }
        }
    }

    private IEnumerator MergeBallsAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Odtwórz dźwięk zlaczenia gracza
        audioManager.Play("ConnectPlayer");

        GameObject[] splittedPlayersObjects = GameObject.FindGameObjectsWithTag("playerSplitted");

        float totalMass = 0f; // Suma mas wszystkich rozdzielonych kulek
        Player originalPlayer = null;

        foreach (GameObject splittedPlayer in splittedPlayersObjects)
        {
            Player splitPlayerComponent = splittedPlayer.GetComponent<Player>();
            if (originalPlayer == null)
                originalPlayer = splittedPlayer.GetComponent<SplitPlayer>().originalPlayer;

            totalMass += splitPlayerComponent.PlayerMass;
            Destroy(splittedPlayer);
        }

        if (originalPlayer != null)
        {
            // Przywróć masę do bazowej + reszta z dzielenia
            originalPlayer.PlayerMass = chunkSize + totalMass;
            originalPlayer.UpdateScale();
        }
    }
}
