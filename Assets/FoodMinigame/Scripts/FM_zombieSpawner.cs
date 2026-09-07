using UnityEngine;

public class FM_ZombieSpawner : MonoBehaviour
{
    public GameObject zombiePrefab;
    public Transform[] spawnPoints;

    public float spawnRate = 1.5f;
    private float nextSpawnTime;

    public FM_GameManager gameManager;

    void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnZombie();
            nextSpawnTime = Time.time + spawnRate;
        }
    }

    void SpawnZombie()
    {
        int index = Random.Range(0, spawnPoints.Length);

        GameObject FM_zombie = Instantiate(
            zombiePrefab,
            spawnPoints[index].position,
            Quaternion.identity
        );

        if (gameManager != null)
            gameManager.ZombieSpawned();
    }
}