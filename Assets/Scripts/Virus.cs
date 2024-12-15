using UnityEngine;
using System.Collections;

/// <summary>
/// 
/// </summary>
public class Virus : MonoBehaviour
{
    /// <summary>
    /// The small circle prefab
    /// </summary>
    public GameObject smallCirclePrefab; // Prefab mniejszych kół
    /// <summary>
    /// The explosion force
    /// </summary>
    public float explosionForce = 5f; // Siła rozrzutu małych kół
    /// <summary>
    /// The pull force
    /// </summary>
    public float pullForce = 1f; // Siła przyciągania do oryginalnego gracza
    /// <summary>
    /// The base stop distance
    /// </summary>
    public float baseStopDistance = 1.5f; // Odległość zatrzymania przyciągania
    /// <summary>
    /// The mass stop scale factor
    /// </summary>
    public float massStopScaleFactor = 0.1f; // Skala odległości zatrzymania na podstawie masy

    /// <summary>
    /// The audio manager
    /// </summary>
    private AudioManager audioManager;

    /// <summary>
    /// Starts this instance.
    /// </summary>
    private void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    /// <summary>
    /// Called when [collision enter2 d].
    /// </summary>
    /// <param name="collision">The collision.</param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.GetComponent<CircleCollider2D>().radius * collision.transform.lossyScale.x);
        Debug.Log(gameObject.GetComponent<CircleCollider2D>().radius * transform.lossyScale.x);
        // Odtwórz dźwięk zetknięcia z wirusem
        audioManager.Play("HitVirus");

        // Sprawdź, czy obiekt, który dotknął wirusa, to gracz
        if (collision.gameObject.CompareTag("Player") &&
            collision.gameObject.GetComponent<GameCircle>().Radius >
            gameObject.GetComponent<GameCircle>().Radius)
        {


            GameObject player = collision.gameObject;
            Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();

            if (playerRb != null)
            {
                Vector3 explosionCenter = player.transform.position;

                GameCircle playerCircle = collision.gameObject.GetComponent<GameCircle>();

                float playerSize = playerCircle.GameCircleSizeScale();
                int numberOfHalvings = (int)Mathf.Log(playerSize, 2);

                // Odtwórz dźwięk podziału gracza
                audioManager.Play("SplitVirus");


                for (int i = 0; i < numberOfHalvings; i++)
                {
                    playerCircle.HalveCircle();
                }
                playerCircle.gameObject.GetComponent<Player>().UpdateScale();

                // Rozdzielenie na mniejsze koła
                for (int i = 0; i < Mathf.Pow(2, numberOfHalvings) - 1; i++)
                {
                    // Stwórz nowe małe kółka
                    GameCircle smallCircle = Instantiate(playerCircle, explosionCenter, Quaternion.identity);
                    smallCircle.PropagateFirstRadius(playerCircle.FirstRadius);
                    smallCircle.gameObject.GetComponent<Player>().UpdateScale();
                    smallCircle.tag = "playerSplitted";


                    // Ustaw ich masę
                    Rigidbody2D rb = smallCircle.GetComponent<Rigidbody2D>();
                    if (rb != null)
                    {
                        rb.mass = 3f;

                        // Dodaj siłę odpychającą
                        Vector2 explosionDirection = (Vector2)(Quaternion.Euler(0, 0, (360f / numberOfHalvings * numberOfHalvings) * i) * Vector2.right);
                        rb.AddForce(explosionDirection * explosionForce, ForceMode2D.Impulse);

                        // Dodaj skrypt do przyciągania do oryginalnej pozycji
                        SplitPlayer splitPlayer = smallCircle.gameObject.AddComponent<SplitPlayer>();
                        splitPlayer.originalPlayer = player.GetComponent<Player>();
                        splitPlayer.pullForce = pullForce;
                        splitPlayer.baseStopDistance = baseStopDistance;
                        splitPlayer.massStopScaleFactor = massStopScaleFactor;
                    }
                }

                // Rozpocznij odliczanie do scalania
                StartCoroutine(MergeBallsAfterDelay(7f));
            }
        }
    }

    /// <summary>
    /// Merges the balls after delay.
    /// </summary>
    /// <param name="delay">The delay.</param>
    /// <returns></returns>
    private IEnumerator MergeBallsAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Odtwórz dźwięk zlaczenia gracza
        audioManager.Play("ConnectPlayer");

        GameObject[] splittedPlayersObjects = GameObject.FindGameObjectsWithTag("playerSplitted");

        Player originalPlayer = null;

        foreach (GameObject splittedPlayer in splittedPlayersObjects)
        {
            GameCircle splitPlayerComponent = splittedPlayer.GetComponent<GameCircle>();
            if (originalPlayer == null)
                originalPlayer = splittedPlayer.GetComponent<SplitPlayer>().originalPlayer;


            originalPlayer.GetComponent<GameCircle>().CombineCircles(splitPlayerComponent);
            Destroy(splittedPlayer);
        }

        if (originalPlayer != null)
        {
            originalPlayer.UpdateScale();
        }
    }
}
