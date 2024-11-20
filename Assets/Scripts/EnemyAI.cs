using UnityEditor;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    // References
    public Transform player; // Reference to the player
    public FoodSpawner foodSpawner;
    public float detectionRange = 20f; // Range within which the enemy detects the player
    public float chaseSpeed = 3f; // Speed at which the enemy chases
    public float patrolSpeed = 1.5f; // Speed at which the enemy patrols
    public float patrolRange = 5f; // Range the enemy moves around while patrolling
    public float stopDistance = 1.5f; // Distance from the player to stop chasing
    public float enemyMass = 5.0f;
    public float foodSeekSpeed = 2f;

    // AI states
    private enum State { Idle, Patrolling, Chasing, Growing }
    private State currentState;

    // Patrol variables
    private Vector2 startPosition; // Starting position of the enemy
    private Vector2 patrolTarget; // Current patrol target
    private bool isPatrolTargetReached = true;
    private Transform foodTarget;


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
            case State.Growing:
                HandleGrowing();
                break;
        }

        CheckPlayerDistance();
        CheckMassDifference();
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

    private void HandleGrowing(){
        
        if (foodTarget == null)
        {
            FindNearestFood();
            return;
        }

        transform.position = Vector2.MoveTowards(transform.position, foodTarget.position, foodSeekSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, foodTarget.position) < 0.1f)
        {
            // Simulate eating the food
            Food food = foodTarget.GetComponent<Food>();
            if (food != null)
            {
                
                //enemyMass += food.FoodMass;
                transform.localScale += Vector3.one * 0.1f;
                float x = Random.Range(28, -28);
                float y = Random.Range(28, 28);
                Vector2 v = new Vector2(x, y);
                food.Relocate(v); // Move the food to a new random position
            }

            foodTarget = null; // Clear the food target
            currentState = State.Patrolling; // Return to patrolling after eating
        }
    }

    private void FindNearestFood()
    {
        float closestDistance = float.MaxValue;
        GameObject closestFood = null;

        foreach (var food in foodSpawner.spawnedFood)
        {
            if (food != null) // Skip null entries
            {
                float distance = Vector2.Distance(transform.position, food.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestFood = food;
                }
            }
        }

        if (closestFood != null)
        {
            foodTarget = closestFood.transform;
        }
        else
        {
            foodTarget = null; // No valid food found
            Debug.LogWarning("No valid food to target!");
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

    private void CheckMassDifference()
    {
        if (enemyMass < player.GetComponent<Player>().transform.lossyScale.x && currentState != State.Growing)
        {
            currentState = State.Growing;
            FindNearestFood();
        }
        else{
            CheckPlayerDistance();
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