using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class PlayerController : MonoBehaviour
{
    /// <summary>
    /// The player
    /// </summary>
    Player player;
    /// <summary>
    /// The food
    /// </summary>
    public GameObject Food;
    /// <summary>
    /// The split distance
    /// </summary>
    private float splitDistance = 0.5f; // Distance the new player will be created from the original player
    /// <summary>
    /// The minimum mass to split
    /// </summary>
    private float minMassToSplit = 10f; // Minimum mass required to allow splitting
    /// <summary>
    /// The split velocity
    /// </summary>
    private float splitVelocity = 10f; // Initial velocity for the split player
    /// <summary>
    /// The food mass
    /// </summary>
    private float foodMass = 0.5f; // Mass of each food dropped
    /// <summary>
    /// The food speed
    /// </summary>
    public float foodSpeed = 2f; // Speed of the dropped food
    /// <summary>
    /// The minimum mass to drop
    /// </summary>
    public float minimumMassToDrop = 10f; // Minimum mass required to drop food

    /// <summary>
    /// The audio manager
    /// </summary>
    private AudioManager audioManager;
    /// <summary>
    /// The main camera
    /// </summary>
    Camera mainCamera;
    /// <summary>
    /// The target orthographic size
    /// </summary>
    private float targetOrthographicSize;
    /// <summary>
    /// The lerp speed
    /// </summary>
    private float lerpSpeed = 2f;

    /// <summary>
    /// Starts this instance.
    /// </summary>
    private void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        mainCamera = Camera.main;
        targetOrthographicSize = mainCamera.orthographicSize;
    }

    /// <summary>
    /// Updates this instance.
    /// </summary>
    private void Update()
    {
        player = GetComponent<Player>();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            SplitPlayer();
        }
        if (Input.GetKeyDown(KeyCode.W)) // Press "W" to drop food
        {
            DropMassAsFood();
        }
        player.UpdateScale();
        mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, targetOrthographicSize, Time.deltaTime * lerpSpeed);
    }

    /// <summary>
    /// Splits the player.
    /// </summary>
    private void SplitPlayer()
    {
        if (player != null)
        {
            Debug.Log("Minimum mass to split: " + minMassToSplit);
            // Check if the player's mass is above the minimum threshold
            if (player.PlayerMass < minMassToSplit)
            {
                Debug.Log("Cannot split: Player mass is too low.");
                return; // Exit the function if the mass is too low
            }

            Debug.Log("PLAYER BEFORE: " + player.PlayerMass.ToString());

            // Get the mouse position in world coordinates
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0; // Ensure the z-coordinate is 0 (2D game scenario)

            // Calculate the direction from the player to the mouse position
            Vector3 direction = (mousePosition - transform.position).normalized;

            // Odtwórz dźwięk podziału gracza
            audioManager.Play("SplitVirus");

            // Instantiate the new player at the specified distance in the direction of the mouse
            Player playerSplitted = Instantiate(player, transform.position + direction * splitDistance, Quaternion.identity);
            player.PlayerMass /= 2f; // Divide mass by 2 for the original player
            playerSplitted.PlayerMass = player.PlayerMass; // Set the mass of the splitted player
            playerSplitted.tag = "playerSplitted";

            // Add Rigidbody2D component if it doesn't already exist
            Rigidbody2D rb = playerSplitted.GetComponent<Rigidbody2D>();
            if (rb == null)
            {
                rb = playerSplitted.gameObject.AddComponent<Rigidbody2D>(); // Add Rigidbody2D if not present
            }

            // Set the initial velocity of the splitted player
            rb.velocity = direction * splitVelocity; // Assign the velocity in the direction of the mouse

            // Attach the SplitPlayer script to the cloned player and set the original player reference
            SplitPlayer splitPlayerScript = playerSplitted.gameObject.AddComponent<SplitPlayer>();
            splitPlayerScript.originalPlayer = player; // Set the reference to the original player

            Debug.Log("PLAYER NOW: " + player.PlayerMass.ToString());
            Debug.Log("SPLITTED: " + playerSplitted.PlayerMass.ToString());

            StartCoroutine(MergeBallsAfterDelay(20f));


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

        foreach (GameObject splittedPlayer in splittedPlayersObjects)
        {
            player.PlayerMass += splittedPlayer.GetComponent<Player>().PlayerMass;
            Destroy(splittedPlayer);
            player.UpdateScale();
        }
    }

    /// <summary>
    /// Drops the mass as food.
    /// </summary>
    private void DropMassAsFood()
    {
        // Check if the player has enough mass to drop food
        if (player.PlayerMass > minimumMassToDrop)
        {
            player.PlayerMass -= foodMass; // Decrease the player's mass by the food's mass

            // Get the mouse position in world space
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // Odtwórz dźwięk zrzucania masy
            audioManager.Play("DropMass");

            Debug.Log($"Food dropped! Player mass is now {player.PlayerMass}");
        }
        else
        {
            Debug.Log("Not enough mass to drop food!");
        }
    }


    /// <summary>
    /// Called when [trigger enter2 d].
    /// </summary>
    /// <param name="other">The other.</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (player.transform.lossyScale.x > other.transform.lossyScale.x)
            {
                EnemyAI enemy = other.gameObject.GetComponent<EnemyAI>();
                player.PlayerScore += (int)enemy.EnemyMass;
                player.PlayerMass += enemy.EnemyMass;
                //Debug.Log("gained: " + enemy.EnemyMass);       
                targetOrthographicSize += player.PlayerMass * 0.001f;
            }
        }
    }

    /// <summary>
    /// Reduces the velocity over time.
    /// </summary>
    /// <param name="rb">The rb.</param>
    /// <returns></returns>
    private IEnumerator ReduceVelocityOverTime(Rigidbody2D rb)
    {
        // Wait for 2 seconds
        yield return new WaitForSeconds(2f);

        // Gradually apply drag to reduce velocity smoothly
        rb.drag = 2f; // Adjust drag value based on how quickly you want it to slow down

        // Optionally, reset the drag value after a while if you don't want it to last forever
        yield return new WaitForSeconds(2f); // Keep drag for an additional 2 seconds
        rb.drag = 0f; // Reset drag after deceleration period
    }
}