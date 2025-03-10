using System.Collections.Generic;
using UnityEngine;

public class LogPrefabs : MonoBehaviour
{
    public GameObject[] logPrefabs;

    public GameObject GetRandomLogPrefab()
    {
        if (logPrefabs == null || logPrefabs.Length == 0)
        {
            Debug.LogWarning("No log prefabs assigned in LogSpawner on " + gameObject.name);
            return null;
        }
        int index = Random.Range(0, logPrefabs.Length);
        return logPrefabs[index];

    }
}
