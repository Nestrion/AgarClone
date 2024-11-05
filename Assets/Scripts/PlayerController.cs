using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Player player;
    private float splitDistance = 0.5f; // Distance the new player will be created from the original player
    private float minMassToSplit = 10f; // Minimum mass required to allow splitting
    private float splitVelocity = 10f; // Initial velocity for the split player

    private void Update()
    {
        player = GetComponent<Player>();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            SplitPlayer();
        }

        player.UpdateScale();
    }

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

    private IEnumerator MergeBallsAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        GameObject[] splittedPlayersObjects = GameObject.FindGameObjectsWithTag("playerSplitted");

        foreach (GameObject splittedPlayer in splittedPlayersObjects)
        {
            player.PlayerMass += splittedPlayer.GetComponent<Player>().PlayerMass;
            Destroy(splittedPlayer);
            player.UpdateScale();
        }
    }
}
