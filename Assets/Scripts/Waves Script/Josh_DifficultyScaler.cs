using UnityEngine;

public class Josh_DifficultyScaler : MonoBehaviour
{
    public float progression = 0f;
    public float speedMultiplier = 1f;
    public int maxEnemies = 5;
    private Josh_WaveManager waveManager;

    void Start()
    {
        waveManager = GetComponent<Josh_WaveManager>();
    }

    public void UpdateDifficulty()
    {
        progression += 0.1f;
        progression = Mathf.Clamp(progression, 0f, 1f);

        // Speed increases up to 2x over the course of the game
        speedMultiplier = Mathf.Lerp(1f, 2f, progression);

        // Enemies per wave increases from 5 up to maxEnemies
        int newEnemiesPerWave = Mathf.RoundToInt(Mathf.Lerp(5, maxEnemies, progression));
        waveManager.enemiesPerWave = newEnemiesPerWave;
    }
}