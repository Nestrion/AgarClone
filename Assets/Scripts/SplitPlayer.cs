using UnityEngine;

/// <summary>
/// 
/// </summary>
public class SplitPlayer : MonoBehaviour
{
    /// <summary>
    /// The original player
    /// </summary>
    public Player originalPlayer; // Reference to the original player
    /// <summary>
    /// The pull force
    /// </summary>
    public float pullForce = 1f; // Strength of the pulling force
    /// <summary>
    /// The base stop distance
    /// </summary>
    public float baseStopDistance = 1.2f; // Base distance at which pulling stops
    /// <summary>
    /// The mass stop scale factor
    /// </summary>
    public float massStopScaleFactor = 1f; // Factor to scale stop distance based on player mass
    /// <summary>
    /// The minimum distance to stop
    /// </summary>
    private float minimumDistanceToStop = 2f; // Minimum distance from the original player after splitting

    /// <summary>
    /// Updates this instance.
    /// </summary>
    private void Update()
    {
        if (originalPlayer != null)
        {
            // Calculate the distance to the original player
            float distanceToOriginal = Vector3.Distance(originalPlayer.transform.position, transform.position);

            // Get the Rigidbody2D component
            Rigidbody2D rb = GetComponent<Rigidbody2D>();

            // Calculate dynamic stop distance based on the original player's mass
            float dynamicStopDistance = baseStopDistance + (originalPlayer.gameObject.transform.lossyScale.x * massStopScaleFactor);

            // Only apply pulling force if the distance is greater than the minimumDistanceToStop
            if (distanceToOriginal > minimumDistanceToStop + (originalPlayer.gameObject.transform.lossyScale.x * massStopScaleFactor))
            {

                // Apply a pulling force towards the original player
                Vector3 direction = (originalPlayer.transform.position - transform.position).normalized;
                if (rb != null)
                {
                    rb.AddForce(direction * pullForce);
                }
            }
            else if (distanceToOriginal <= dynamicStopDistance)
            {
                // Stop the player when within the dynamic stop distance and moving toward the original player
                if (rb != null)
                {
                    Vector3 directionToPlayer = (originalPlayer.transform.position - transform.position).normalized;

                    // Check if the velocity is moving toward the original player
                    if (Vector3.Dot(rb.velocity.normalized, directionToPlayer) > 0)
                    {
                        rb.velocity = Vector2.zero; // Stop all movement
                    }
                }
            }
        }
    }
}
