using UnityEngine;
using static SpawnPointManager;

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

        direction = (transform.position.x > 0) ? Direction.left : Direction.right;
    }

    private void Update()
    {
        if (nextSpawnTime <= Time.time)
        {
            if (CanSpawn())
            {
                SpawnObstacle(obstacleType);
            }
            nextSpawnTime = Time.time + spawnRate;
        }
    }

    private bool CanSpawn()
    {
        if (lastSpawnedObstacle == null)
            return true;

        Collider2D collision = lastSpawnedObstacle.GetComponent<Collider2D>();
        if (collision == null)
            return true;

        float distance = Vector2.Distance(transform.position, lastSpawnedObstacle.transform.position);
        float obstacleWidth = collision.bounds.size.x;
        return distance >= minSpacing + obstacleWidth;
    }

    private void SpawnObstacle(ObstacleType type)
    {
        GameObject selectedPrefab = spawnPointManager.GetRandomPrefab(type);
        GameObject newObstacle = Instantiate(selectedPrefab, transform.position, Quaternion.identity);

        ObstacleMovement movement = newObstacle.GetComponent<ObstacleMovement>();


        if (movement != null)
        {
            movement.direction = (direction == Direction.right) ? Vector2.right : Vector2.left;
            movement.speed = laneSpeed;

            if (movement.direction == Vector2.left) 
            {
                newObstacle.transform.localRotation = Quaternion.Euler(0, 180, 0);
            }
        }
        lastSpawnedObstacle = newObstacle;
    }
}
