using UnityEngine;

public class Josh_EnemyTypeManager : MonoBehaviour
{
    public GameObject[] enemyTypes;
    private Josh_DifficultyScaler difficultyScaler;

    // Change Start() to Awake()
    void Awake()
    {
        difficultyScaler = GetComponent<Josh_DifficultyScaler>();
    }

    public void SpawnRandomEnemy(Vector2 position)
    {
        if (enemyTypes.Length == 0) return;

        GameObject enemyPrefab = PickEnemyType();
        GameObject enemy = Instantiate(enemyPrefab, position, Quaternion.identity);
        
        Josh_EnemyMovement360 movement = enemy.GetComponent<Josh_EnemyMovement360>();
        if (movement != null)
        {
            movement.speed = movement.baseSpeed * difficultyScaler.speedMultiplier;
            movement.baseSpeed = movement.baseSpeed * difficultyScaler.speedMultiplier;
        }
    }

    GameObject PickEnemyType()
    {
        float progression = difficultyScaler.progression;

        if (enemyTypes.Length == 1 || progression < 0.3f)
        {
            return enemyTypes[0];
        }
        else if (enemyTypes.Length == 2 || progression < 0.6f)
        {
            int index = Random.value > 0.5f ? 1 : 0;
            return enemyTypes[index];
        }
        else
        {
            return enemyTypes[Random.Range(0, enemyTypes.Length)];
        }
    }
}