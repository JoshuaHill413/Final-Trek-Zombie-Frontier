using UnityEngine;

public class Josh_GameResultData : MonoBehaviour
{
    public static Josh_GameResultData Instance;

    public bool isWin;
    public int resourceAmount;
    public string resourceType;

    // Tracks which minigames have been played this city
    public bool scavengePlayed = false;
    public bool gasPlayed = false;

    // Tracks current city so we can reset when city changes
    public int lastCity = -1;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        // Always check on start if city has changed
        GameData.Load();
        ResetForNewCity(GameData.CityIndex);
    }

    public void ResetForNewCity(int newCity)
    {
        if (lastCity != newCity)
        {
            Debug.Log($"New city reached: {newCity}, resetting minigame flags.");
            scavengePlayed = false;
            gasPlayed      = false;
            lastCity       = newCity;
        }
    }
}