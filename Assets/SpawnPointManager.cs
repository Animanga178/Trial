using System;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointManager : MonoBehaviour
{
    public static SpawnPointManager Instance { get; private set; }

    public enum ObstacleType { Log }

    [Serializable]
    public class ObstaclePrefab
    {
        public ObstacleType type;
        public List<GameObject> prefabs;
    }

    [Serializable]
    public class SpawnPointConfig
    {
        public ObstacleType type;
        public List<GameObject> gameObjects;
    }

    public List<ObstaclePrefab> obstaclePrefabs = new List<ObstaclePrefab>();
    public List<SpawnPointConfig> spawnPointConfigs = new List<SpawnPointConfig>();

    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        // Initalise dictionaries
        obstaclePrefabs.Add(new ObstaclePrefab { type = ObstacleType.Log, prefabs = new List<GameObject>() });

        // Accessing spawn point by obstacle type
        spawnPointConfigs.Add(new SpawnPointConfig { type = ObstacleType.Log, gameObjects = new List<GameObject>() });
    }

    public GameObject GetRandomPrefab(ObstacleType type)
    {
        ObstaclePrefab prefab = obstaclePrefabs.Find(p => p.type == type);
        if (prefab == null || prefab.prefabs.Count == 0)
        {
            Debug.LogWarning("No prefabs assigned for type " + type);
            return null;
        }

        int index = UnityEngine.Random.Range(0, prefab.prefabs.Count);
        return prefab.prefabs[index];
    }
}

public class LogSpawner : MonoBehaviour
{
    private SpawnPointManager spawnPointManager;

    private void Start()
    {
        spawnPointManager = SpawnPointManager.Instance;
    }

    public GameObject SpawnObstacle()
    {
        return spawnPointManager.GetRandomPrefab(SpawnPointManager.ObstacleType.Log);
    }
}

