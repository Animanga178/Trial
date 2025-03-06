using UnityEngine;

public class SpawnPointManager : MonoBehaviour
{
    public Transform[] spawnPoints;
   
    public Transform GetSpawnPoint(int index)
    {
        return spawnPoints[index];
    }
}
