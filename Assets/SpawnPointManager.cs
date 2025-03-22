using System;
using System.Collections.Generic;
using UnityEngine;

// Manage spawn points and obstacle prefabs
public class SpawnPointManager : MonoBehaviour
{
    // Singleton instance of the SpawnPointManager
    public static SpawnPointManager Instance { get; private set; }

    public enum ObstacleType { Log, Car }

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
        // List of game objects for the spawn point
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
            DontDestroyOnLoad(gameObject);
        }
    }

    // Returns a random prefab for the specified obstacle type
    public GameObject GetRandomPrefab(ObstacleType type)
    {
        // Find the obstacle prefab for the specified type
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