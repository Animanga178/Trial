using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public float spawnRate = 2f;
    public float minSpacing = 2f;
    public float nextSpawnTime = 0f;

    public Vector2 direction = Vector2.right;
    public GameObject lastSpawnedObstacle;
    public float laneSpeed = 10f;

    public LogSpawner logSpawner;

    private void Update()
    {
        if (Time.time >= nextSpawnTime)
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
        GameObject selectedPrefab = logSpawner.GetRandomLogPrefab();
        GameObject newObstacle = Instantiate(selectedPrefab, transform.position, Quaternion.identity);

        ObstacleMovement movement = newObstacle.GetComponent<ObstacleMovement>();

        if (movement != null)
        {
            movement.direction = direction;
            movement.speed = laneSpeed;
        }

        lastSpawnedObstacle = newObstacle;
    }
}
