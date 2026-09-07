using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
 
public class FM_GameManager : MonoBehaviour
{
    [Header("Timer")]
    public float timeLeft = 30f;
    private bool spawningStopped = false;
    private bool gameEnded = false;
 
    [Header("UI")]
    public TextMeshProUGUI timerText;
    public GameObject resultPanel;
    public GameObject winText;
    public GameObject loseText;
    public TextMeshProUGUI promptText;
 
    [Header("Spawner")]
    public FM_ZombieSpawner spawner;
 
    [Header("Difficulty")]
    public int currentRound = 1;
    public float spawnRateDecrease = 0.2f;
    public float zombieSpeedIncrease = 0.3f;
    public float minSpawnRate = 0.3f;
 
    [Header("Results")]
    public string resultsScene = "Results";
    public int foodRewardMin = 10;
    public int foodRewardMax = 30;
    public int foodLossMin = 5;
    public int foodLossMax = 20;
 
    private int zombiesAlive = 0;
    private bool waitingForInput = false;
 
    void Start()
    {
        // Ensure Josh_GameResultData exists even if we came straight to this scene
        if (Josh_GameResultData.Instance == null)
        {
            GameObject obj = new GameObject("Josh_GameResultData");
            obj.AddComponent<Josh_GameResultData>();
        }
 
        if (resultPanel != null) resultPanel.SetActive(false);
        if (winText     != null) winText.SetActive(false);
        if (loseText    != null) loseText.SetActive(false);
        if (promptText  != null) promptText.gameObject.SetActive(false);
 
        ApplyDifficulty();
    }
 
    void Update()
    {
        if (gameEnded)
        {
            if (waitingForInput && Keyboard.current.enterKey.wasPressedThisFrame)
            {
                Time.timeScale = 1f;
                SceneManager.LoadScene(resultsScene);
            }
            return;
        }
 
        if (!spawningStopped)
        {
            timeLeft -= Time.deltaTime;
 
            if (timerText != null)
                timerText.text = "TIME: " + Mathf.Ceil(timeLeft);
 
            if (timeLeft <= 0)
                StopSpawning();
        }
 
        if (spawningStopped && zombiesAlive <= 0)
            WinGame();
    }
 
    void ApplyDifficulty()
    {
        if (spawner != null)
        {
            spawner.spawnRate = Mathf.Max(
                minSpawnRate,
                spawner.spawnRate - (spawnRateDecrease * (currentRound - 1))
            );
        }
 
        FM_Zombie[] zombies = FindObjectsByType<FM_Zombie>(FindObjectsSortMode.None);
        foreach (FM_Zombie zombie in zombies)
            zombie.speed += zombieSpeedIncrease * (currentRound - 1);
    }
 
    void StopSpawning()
    {
        spawningStopped = true;
        if (spawner != null)
            spawner.enabled = false;
    }
 
    public void ZombieSpawned() { zombiesAlive++; }
 
    public void ZombieKilled()
    {
        zombiesAlive--;
        if (zombiesAlive < 0) zombiesAlive = 0;
    }
 
    public void LoseGame()
    {
        if (gameEnded) return;
        gameEnded = true;
 
        int lossAmount = Random.Range(foodLossMin, foodLossMax + 1);
 
        if (Josh_GameResultData.Instance != null)
        {
            Josh_GameResultData.Instance.isWin          = false;
            Josh_GameResultData.Instance.resourceAmount = lossAmount;
            Josh_GameResultData.Instance.resourceType   = "Food";
            Josh_GameResultData.Instance.scavengePlayed = true;
        }
 
        if (resultPanel != null) resultPanel.SetActive(true);
        if (loseText    != null) loseText.SetActive(true);
        if (promptText  != null) promptText.gameObject.SetActive(true);
 
        Time.timeScale = 0f;
        waitingForInput = true;
    }
 
    void WinGame()
    {
        if (gameEnded) return;
        gameEnded = true;
 
        int rewardAmount = Random.Range(foodRewardMin, foodRewardMax + 1);
 
        if (Josh_GameResultData.Instance != null)
        {
            Josh_GameResultData.Instance.isWin          = true;
            Josh_GameResultData.Instance.resourceAmount = rewardAmount;
            Josh_GameResultData.Instance.resourceType   = "Food";
            Josh_GameResultData.Instance.scavengePlayed = true;
        }
 
        if (resultPanel != null) resultPanel.SetActive(true);
        if (winText     != null) winText.SetActive(true);
        if (promptText  != null) promptText.gameObject.SetActive(true);
 
        Time.timeScale = 0f;
        waitingForInput = true;
    }
}
