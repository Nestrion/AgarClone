using UnityEngine;

public class PlayerMovements : MonoBehaviour
{
    public float speed = 4f;

    // Distance from the player in screen space at which player achieves max speed
    [SerializeField, Range(0.1f, 1)] private float maxSpeedRadius = 0.25f;

    void Update()
    {
        Move();
    }

    void Move()
    {
        Camera cam = Camera.main;
        Vector3 worldInput = cam.ScreenToWorldPoint(Input.mousePosition);

        // values get clamped because viewport values can go beyond 1 and under 0 
        // both have player.z position, so the distance of the vector doesn't get affected by the z axis
        Vector3 playerViewportPos = cam.WorldToViewportPoint(transform.position);
        playerViewportPos = new Vector3(Mathf.Clamp(playerViewportPos.x, 0, 1), Mathf.Clamp(playerViewportPos.y, 0, 1), playerViewportPos.z);

        Vector3 mouseViewportPos = cam.ScreenToViewportPoint(Input.mousePosition);
        mouseViewportPos = new Vector3(Mathf.Clamp(mouseViewportPos.x, 0, 1), Mathf.Clamp(mouseViewportPos.y, 0, 1), playerViewportPos.z);


        float viewportDistance = Mathf.Min(Vector2.Distance(playerViewportPos, mouseViewportPos), maxSpeedRadius) / maxSpeedRadius;
        float finalSpeed = speed * viewportDistance;
        Debug.Log(finalSpeed);

        // seems like MoveTowards takes into account the distance and it changes the speed, even if speed doesnt change
        // Vector3 newPosition = Vector3.MoveTowards(transform.position, worldInput, finalSpeed * Time.deltaTime);
        Vector3 newPosition = transform.position + (worldInput - transform.position).normalized * finalSpeed * Time.deltaTime;

        newPosition.z = transform.position.z;
        transform.position = newPosition;
    }
}
