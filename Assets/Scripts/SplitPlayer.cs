using UnityEditor.Callbacks;
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
    public float baseStopDistance = 1.1f; // Base distance at which pulling stops
    /// <summary>
    /// The mass stop scale factor
    /// </summary>
    public float massStopScaleFactor = 1.2f; // Factor to scale stop distance based on player mass
    /// <summary>
    /// The minimum distance to stop
    /// </summary>
    private float minimumDistanceToStop = 2f; // Minimum distance from the original player after splitting

    private int collisionsEnteredSoFar = 0;

    private bool pullToOriginalPlayer = true;

    private bool fliedAway = false;

    /// <summary>
    /// Updates this instance.
    /// </summary>
    private void Update()
    {
        if (originalPlayer != null)
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            Debug.Log("velo : " + rb.totalForce.magnitude);
            float distanceToOriginal = Vector3.Distance(originalPlayer.transform.position, transform.position);

            if (distanceToOriginal > originalPlayer.GetComponent<GameCircle>().GameCircleSizeScale() * 2.1f)
                fliedAway = true;

            if (distanceToOriginal > originalPlayer.GetComponent<GameCircle>().GameCircleSizeScale() * 2.1f &&
                rb.velocity.magnitude < 0.3f)
            {

                if (pullToOriginalPlayer == true)
                {
                    Vector3 direction = (originalPlayer.transform.position - transform.position).normalized;
                    if (rb != null)
                    {
                        rb.velocity = direction * pullForce;
                    }
                }
                else
                {
                    rb.velocity = Vector2.zero;
                }
            }


        }
    }


    private void OnCollisionStay2D(Collision2D other)
    {
        if (fliedAway)
        {
            if (other.gameObject.GetComponent<Player>())
            {
                pullToOriginalPlayer = false;
                Debug.Log("we done it aight");
            }
        }
    }

}
