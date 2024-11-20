using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    // References
    public Transform player; // Reference to the player
    public float detectionRange = 10f; // Range within which the enemy detects the player
    public float chaseSpeed = 3f; // Speed at which the enemy chases
    public float patrolSpeed = 1.5f; // Speed at which the enemy patrols
    public float patrolRange = 25f; // Range the enemy moves around while patrolling
    public float stopDistance = 1.5f; // Distance from the player to stop chasing

    // AI states
    private enum State { Idle, Patrolling, Chasing }
    private State currentState;

    // Patrol variables
    private Vector2 startPosition; // Starting position of the enemy
    private Vector2 patrolTarget; // Current patrol target
    private bool isPatrolTargetReached = true;

    void Start()
    {
        currentState = State.Idle;
        startPosition = transform.position;
        SetNewPatrolTarget();
    }

    void Update()
    {
        switch (currentState)
        {
            case State.Idle:
                HandleIdle();
                break;
            case State.Patrolling:
                HandlePatrolling();
                break;
            case State.Chasing:
                HandleChasing();
                break;
        }

        CheckPlayerDistance();
    }

    private void HandleIdle()
    {
        // Remain idle until a condition (like time or player proximity) triggers a state change
        if (Vector2.Distance(transform.position, startPosition) > 0.1f)
        {
            transform.position = Vector2.MoveTowards(transform.position, startPosition, patrolSpeed * Time.deltaTime);
        }
    }

    private void HandlePatrolling()
    {
        if (isPatrolTargetReached)
        {
            SetNewPatrolTarget();
        }

        // Move towards the patrol target
        transform.position = Vector2.MoveTowards(transform.position, patrolTarget, patrolSpeed * Time.deltaTime);

        // Check if the target has been reached
        if (Vector2.Distance(transform.position, patrolTarget) < 0.1f)
        {
            isPatrolTargetReached = true;
        }
    }

    private void HandleChasing()
    {
        if (Vector2.Distance(transform.position, player.position) > stopDistance)
        {
            // Move towards the player
            transform.position = Vector2.MoveTowards(transform.position, player.position, chaseSpeed * Time.deltaTime);
        }
        else
        {
            // Stop when close enough
            currentState = State.Idle;
        }
    }

    private void CheckPlayerDistance()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            currentState = State.Chasing;
        }
        else if (currentState == State.Chasing && distanceToPlayer > detectionRange)
        {
            currentState = State.Patrolling;
        }
    }

    private void SetNewPatrolTarget()
    {
        isPatrolTargetReached = false;

        // Generate a new random patrol target within patrolRange
        patrolTarget = startPosition + new Vector2(
            Random.Range(-patrolRange, patrolRange),
            Random.Range(-patrolRange, patrolRange)
        );
    }

    // Optional: Debugging to visualize ranges
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(startPosition, patrolRange);
    }
}