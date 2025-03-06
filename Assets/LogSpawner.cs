using System.Collections.Generic;
using UnityEngine;

public class LogSpawner : MonoBehaviour
{
    public List<GameObject> logPrefabs;

    public GameObject GetRandomLogPrefab()
    {
        if (logPrefabs == null || logPrefabs.Count == 0)
        {
            Debug.LogWarning("No log prefabs assigned in LogSpawner on " + gameObject.name);
            return null; 
        }
        int index = Random.Range(0, logPrefabs.Count);
        return logPrefabs[index];

    }
}
