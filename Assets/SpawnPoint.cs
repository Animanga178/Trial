using UnityEngine;
using static SpawnPointManager;

// Manages the spawning of obstacles at a specific point
public class SpawnPoint : MonoBehaviour
{
    [SerializeField] private float spawnRate = 2f;
    [SerializeField] private float minSpacing = 2f;
    private float nextSpawnTime = 0f;

    private enum Direction { left, right };
    private Direction direction;
    private GameObject lastSpawnedObstacle;
    [SerializeField] private float laneSpeed = 10f;

    private SpawnPointManager spawnPointManager;
    private SpawnPointManager.ObstacleType obstacleType;

    private void Start()
    {
        spawnPointManager = SpawnPointManager.Instance;

        // Get the SpawnPointConfig for this spawn point
        SpawnPointConfig config = spawnPointManager.spawnPointConfigs.Find(c => c.gameObjects.Contains(gameObject));

        // Get the ObstacleType from the config
        obstacleType = config.type;

        // Determine the direction based on the spawn point's position
        direction = (transform.position.x > 0) ? Direction.left : Direction.right;
    }

    private void Update()
    {
        if (ObstacleMovement.GlobalFreeze && obstacleType == SpawnPointManager.ObstacleType.Log) return;

        if (nextSpawnTime <= Time.time)
        {
            if (CanSpawn())
            {
                Debug.Log($"[{gameObject.name}] GlobalFreeze: {ObstacleMovement.GlobalFreeze}, CanSpawn: {CanSpawn()}");

                SpawnObstacle(obstacleType);
            }
            nextSpawnTime = Time.time + spawnRate;
        }
    }

    // Checks if it's possible to spawn a new obstacle
    private bool CanSpawn()
    {
        if (lastSpawnedObstacle == null)
            return true;

        Collider2D collision = lastSpawnedObstacle.GetComponent<Collider2D>();
        if (collision == null)
            return true;

        float distance = Vector2.Distance(transform.position, lastSpawnedObstacle.transform.position);
        float obstacleWidth = collision.bounds.size.x;

        // Returns if the distance is greater than or equal to the minimum space + the obstacle width
        return distance >= minSpacing + obstacleWidth;
    }

    // Spawns a new obstacle
    private void SpawnObstacle(ObstacleType type)
    {
        // Get a random prefab for the specified obstacle type
        GameObject selectedPrefab = spawnPointManager.GetRandomPrefab(type);
        if (selectedPrefab == null)
        {
            Debug.LogWarning($"[{gameObject.name}] No prefab found for type {type}!");
            return;
        }
        GameObject newObstacle = Instantiate(selectedPrefab, transform.position, Quaternion.identity);

        // Get the ObstacleMovement script for the new obstacle
        ObstacleMovement movement = newObstacle.GetComponent<ObstacleMovement>();

        // Sets the direction and speed with the Obstacle Movement script
        if (movement != null)
        {
            movement.direction = (direction == Direction.right) ? Vector2.right : Vector2.left;
            movement.speed = laneSpeed;

            // If the obstacle is moving left, then it is flipped
            if (movement.direction == Vector2.left) 
            {
                newObstacle.transform.localRotation = Quaternion.Euler(0, 180, 0);
            }

            if (ObstacleMovement.GlobalFreeze && newObstacle.CompareTag("OnWater"))
            {
                movement.Freeze();
            }
        }
        lastSpawnedObstacle = newObstacle;
    }
}
