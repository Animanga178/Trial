using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public float spawnRate = 2f;
    public float minSpacing = 2f;
    public float nextSpawnTime = 0f;

    public enum Direction { left, right };
    public Direction direction;
    public GameObject lastSpawnedObstacle;
    public float laneSpeed = 10f;

    public SpawnPointManager spawnPointManager;

    private void Start()
    {
        spawnPointManager = SpawnPointManager.Instance;

        direction = (transform.position.x > 0) ? Direction.left : Direction.right;
    }

    private void Update()
    {
        if (nextSpawnTime <= Time.time)
        {
            if (CanSpawn())
            {
                SpawnObstacle();
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
        float logWidth = collision.bounds.size.x;
        return distance >= minSpacing + logWidth;
    }

    private void SpawnObstacle()
    {
        GameObject selectedPrefab = spawnPointManager.GetRandomPrefab(SpawnPointManager.ObstacleType.Log);
        GameObject newObstacle = Instantiate(selectedPrefab, transform.position, Quaternion.identity);

        ObstacleMovement movement = newObstacle.GetComponent<ObstacleMovement>();


        if (movement != null)
        {
            movement.direction = (direction == Direction.right) ? Vector2.right : Vector2.left;
            movement.speed = laneSpeed;
        }
        lastSpawnedObstacle = newObstacle;
    }
}
