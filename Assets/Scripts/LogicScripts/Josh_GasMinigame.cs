using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
 
public class Josh_GasMinigame : MonoBehaviour
{
    [Header("Game Settings")]
    public float gameTime = 90f;
    public int difficulty = 1;
    public bool gameActive = false;
 
    [Header("References")]
    public Josh_WaveManager waveManager;
    public Josh_DifficultyScaler difficultyScaler;
    public Josh_GasRewardSystem gasRewardSystem;
    public Josh_AutoFireController autoFireController;
 
    [Header("Audio")]
    public AudioSource backgroundMusic;
    public AudioSource resultMusic;
    public float musicStartTime = 10f;
    public AudioClip winSong;
    public AudioClip loseSong;
    public AudioClip[] gruntSounds;
 
    [Header("UI")]
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI introText;
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI promptText;
 
    [Header("Scenes")]
    public string resultsScene = "Results";
 
    private float currentTime;
    private bool gameEnded = false;
 
    void Start()
    {
        // Ensure Josh_GameResultData exists even if we came straight to this scene
        if (Josh_GameResultData.Instance == null)
        {
            GameObject obj = new GameObject("Josh_GameResultData");
            obj.AddComponent<Josh_GameResultData>();
        }
 
        if (backgroundMusic != null)
        {
            backgroundMusic.time = musicStartTime;
            backgroundMusic.Play();
        }
 
        StartCoroutine(GruntLoop());
 
        currentTime = gameTime;
        promptText.gameObject.SetActive(false);
        UpdateTimerDisplay();
        StartCoroutine(StartGameSequence());
        InvokeRepeating("IncreaseDifficulty", 20f, 20f);
    }
 
    void Update()
    {
        if (!gameActive)
        {
            if (gameEnded && Keyboard.current.enterKey.wasPressedThisFrame)
            {
                SceneManager.LoadScene(resultsScene);
            }
            return;
        }
 
        currentTime -= Time.deltaTime;
        UpdateTimerDisplay();
 
        if (currentTime <= 0)
        {
            currentTime = 0;
            UpdateTimerDisplay();
            EndGame(true);
        }
    }
 
    IEnumerator StartGameSequence()
    {
        introText.gameObject.SetActive(true);
        if (autoFireController != null) autoFireController.SetFiring(false);
 
        yield return new WaitForSeconds(3f);
 
        introText.gameObject.SetActive(false);
        gameActive = true;
        if (autoFireController != null) autoFireController.SetFiring(true);
        waveManager.StartSpawning();
    }
 
    void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60f);
        int seconds = Mathf.FloorToInt(currentTime % 60f);
        timerText.text = string.Format("{0}:{1:00}", minutes, seconds);
    }
 
    public void EndGame(bool win)
    {
        if (!gameActive) return;
 
        gameActive = false;
        gameEnded = true;
        waveManager.StopSpawning();
        if (autoFireController != null) autoFireController.SetFiring(false);
        CancelInvoke("IncreaseDifficulty");
        StopCoroutine(GruntLoop());
 
        if (backgroundMusic != null) backgroundMusic.Stop();
        if (resultMusic != null)
        {
            resultMusic.clip = win ? winSong : loseSong;
            resultMusic.Play();
        }
 
        if (Josh_GameResultData.Instance == null) return;
 
        // Mark gas minigame as played regardless of win or lose
        Josh_GameResultData.Instance.gasPlayed = true;
 
        resultText.gameObject.SetActive(true);
        promptText.gameObject.SetActive(true);
 
        if (win)
        {
            int gasGained = gasRewardSystem.GetGasReward();
 
            Josh_GameResultData.Instance.isWin          = true;
            Josh_GameResultData.Instance.resourceAmount = gasGained;
            Josh_GameResultData.Instance.resourceType   = "Gas";
 
            resultText.text = "You survived!";
            promptText.text = "Press Enter to Continue";
        }
        else
        {
            var lossInfo = gasRewardSystem.GetScaledLoss();
 
            Josh_GameResultData.Instance.isWin          = false;
            Josh_GameResultData.Instance.resourceAmount = lossInfo.amount;
            Josh_GameResultData.Instance.resourceType   = lossInfo.resource;
 
            resultText.text = "A zombie reached your car!";
            promptText.text = "Press Enter to Continue";
        }
    }
 
    IEnumerator GruntLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(10f);
 
            if (gameActive && gruntSounds.Length > 0)
            {
                AudioClip grunt = gruntSounds[Random.Range(0, gruntSounds.Length)];
                AudioSource.PlayClipAtPoint(grunt, Camera.main.transform.position, 1f);
            }
        }
    }
 
    void IncreaseDifficulty()
    {
        if (!gameActive) return;
        difficulty++;
        difficultyScaler.UpdateDifficulty();
    }
}
