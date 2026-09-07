using UnityEngine;
using System.Collections;

public class Josh_WaveManager : MonoBehaviour
{
    public float spawnRadius = 8f;
    public int enemiesPerWave = 5;
    public float timeBetweenWaves = 5f;
    public float timeBetweenSpawns = 0.5f;
    private bool isSpawning = false;
    private Josh_EnemyTypeManager enemyTypeManager;

    void Start()
{
    enemyTypeManager = GetComponent<Josh_EnemyTypeManager>();
}

    public void StartSpawning()
    {
        if (!isSpawning)
        {
            isSpawning = true;
            StartCoroutine(SpawnLoop());
        }
    }

    public void StopSpawning()
    {
        isSpawning = false;
        StopAllCoroutines();
    }

    IEnumerator SpawnLoop()
    {
        while (isSpawning)
        {
            yield return StartCoroutine(SpawnWave());
            yield return new WaitForSeconds(timeBetweenWaves);
        }
    }

    IEnumerator SpawnWave()
    {
        for (int i = 0; i < enemiesPerWave; i++)
        {
            Vector2 spawnPos = GetSpawnPosition();
            enemyTypeManager.SpawnRandomEnemy(spawnPos);
            yield return new WaitForSeconds(timeBetweenSpawns);
        }
    }

    public Vector2 GetSpawnPosition()
{
    // Offset pushes spawns just outside the visible map edge
    float outsideOffset = 1.5f;

    // Pick a random spawn zone - 0=left, 1=right, 2=bottom, 3=door
    int zone = Random.Range(0, 4);

    switch (zone)
    {
        case 0: // Left side - full height below building
            return new Vector2(
                -9f - outsideOffset,
                Random.Range(-5f, 4.3f)
            );

        case 1: // Right side - full height below building
            return new Vector2(
                9f + outsideOffset,
                Random.Range(-5f, 4.3f)
            );

        case 2: // Bottom - full width
            return new Vector2(
                Random.Range(-9f, 9f),
                -6f - outsideOffset
            );

        case 3: // Door at top - only between x 4.32 and 8.84
            return new Vector2(
                Random.Range(4.32f, 8.84f),
                4.3f + outsideOffset
            );

        default:
            return Vector2.zero;
    }
}
}