//using System;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class EnemyAI : MonoBehaviour
{
    // References
    /// <summary>
    /// The player
    /// </summary>
    public Transform player; // Reference to the player
    /// <summary>
    /// The detection range
    /// </summary>
    public float detectionRange = 10f; // Range within which the enemy detects the player
    /// <summary>
    /// The chase speed
    /// </summary>
    public float chaseSpeed = 3f; // Speed at which the enemy chases
    /// <summary>
    /// The patrol speed
    /// </summary>
    public float patrolSpeed = 1.5f; // Speed at which the enemy patrols
    /// <summary>
    /// The patrol range
    /// </summary>
    public float patrolRange = 25f; // Range the enemy moves around while patrolling
    /// <summary>
    /// The stop distance
    /// </summary>
    public float stopDistance = 0.1f; // Distance from the player to stop chasing
    /// <summary>
    /// The closest food
    /// </summary>
    public GameObject closestFood;

    /// <summary>
    /// The growth factor
    /// </summary>
    public float growthFactor = 0.05f;
    /// <summary>
    /// The enemy mass
    /// </summary>
    public float EnemyMass = 0.05f;

    /// <summary>
    /// The enemy mass
    /// </summary>
    public int EnemyScore = 10;

    // AI states
    /// <summary>
    /// 
    /// </summary>
    private enum State { Idle, Patrolling, Chasing, Growing }
    /// <summary>
    /// The current state
    /// </summary>
    private State currentState;

    // Patrol variables
    /// <summary>
    /// The start position
    /// </summary>
    private Vector2 startPosition; // Starting position of the enemy
    /// <summary>
    /// The patrol target
    /// </summary>
    private Vector2 patrolTarget; // Current patrol target
    /// <summary>
    /// The is patrol target reached
    /// </summary>
    private bool isPatrolTargetReached = true;

    /// <summary>
    /// The food spawner
    /// </summary>
    public FoodSpawner foodSpawner;

    /// <summary>
    /// Starts this instance.
    /// </summary>
    void Start()
    {
        currentState = State.Growing;
        startPosition = transform.position;
        SetNewPatrolTarget();
        UI_Manager.Instance.table.AddEntry(name);
        UI_Manager.Instance.table.UpdateEntryByPlayerName(name, EnemyScore);
    }

    /// <summary>
    /// Updates this instance.
    /// </summary>
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

        UI_Manager.Instance.table.UpdateEntryByPlayerName(name, EnemyScore);
    }

    /// <summary>
    /// Handles the idle.
    /// </summary>
    private void HandleIdle()
    {
        // Remain idle until a condition (like time or player proximity) triggers a state change
        if (Vector2.Distance(transform.position, startPosition) > 0.1f)
        {
            transform.position = Vector2.MoveTowards(transform.position, startPosition, patrolSpeed * Time.deltaTime);
        }
    }

    /// <summary>
    /// Handles the growing.
    /// </summary>
    private void HandleGrowing()
    {
        FindClosestFood();

        transform.position = Vector2.MoveTowards(transform.position, closestFood.transform.position, patrolSpeed * Time.deltaTime);

    }

    /// <summary>
    /// Handles the patrolling.
    /// </summary>
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

    /// <summary>
    /// Handles the chasing.
    /// </summary>
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

    /// <summary>
    /// Checks the player distance and decide what action to take.
    /// </summary>
    private void CheckPlayerDistance()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange && transform.lossyScale.x > player.lossyScale.x)
        {
            currentState = State.Chasing;
        }
        if (distanceToPlayer <= detectionRange && transform.lossyScale.x < player.lossyScale.x)
        {
            currentState = State.Growing;
        }
        else if (currentState == State.Chasing && distanceToPlayer > detectionRange)
        {
            currentState = State.Patrolling;
        }

        else if (currentState == State.Patrolling && transform.lossyScale.x < player.lossyScale.x)
        {
            currentState = State.Growing;
        }

        else if (currentState == State.Growing && transform.lossyScale.x >= player.lossyScale.x)
        {
            currentState = State.Patrolling;
        }


    }

    /// <summary>
    /// Sets the new patrol target.
    /// </summary>
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
    /// <summary>
    /// Called when [draw gizmos].
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(startPosition, patrolRange);
    }

    /// <summary>
    /// Finds the closest food.
    /// </summary>
    private void FindClosestFood()
    {


        float closestDistance = float.MaxValue;
        //GameObject closestFood;

        foreach (var food in foodSpawner.spawnedFood)
        {
            if (food != null)
            {
                float distance = Vector2.Distance(transform.position, food.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestFood = food;
                }
            }
        }
    }

    /// <summary>
    /// Called when [trigger enter2 d].
    /// </summary>
    /// <param name="other">The other.</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Food>())
        {
            // Increase the enemy's mass and size
            Food food = other.GetComponent<Food>();
            if (food != null)
            {

                GameCircle enemyCircle = GetComponent<GameCircle>();
                GameCircle foodCircle = food.GetComponent<GameCircle>();
                enemyCircle.CombineCircles(foodCircle);

                // Grow the enemy visually
                EnemyScore += 1;

                UpdateScale();

                // Relocate the food
                Vector2 newPosition = foodSpawner.GetRandomPosition();
                food.Relocate(newPosition);
            }
        }

        if (other.GetComponent<Player>())
        {
            if (transform.lossyScale.x < player.transform.lossyScale.x)
            {
                Destroy(gameObject);
            }
            else if (transform.lossyScale.x > player.transform.lossyScale.x)
            {

                EnemyScore += other.GetComponent<Player>().PlayerScore;
                other.GetComponent<Player>().PlayerScore = 0;

                GameCircle enemyCircle = GetComponent<GameCircle>();
                GameCircle playerCircle = player.GetComponent<GameCircle>();
                enemyCircle.CombineCircles(playerCircle);

                UpdateScale();
            }
        }
    }

    public void UpdateScale()
    {
        GameCircle gameCircle = GetComponent<GameCircle>();
        if (gameCircle != null)
        {
            gameCircle.transform.localScale = new Vector3(gameCircle.GameCircleSizeScale(),
                                                          gameCircle.GameCircleSizeScale(),
                                                          gameCircle.GameCircleSizeScale());
        }
        else
        {
            Debug.Log("no game circle");
        }
    }
}