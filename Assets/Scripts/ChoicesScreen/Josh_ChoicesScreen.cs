using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;
 
public class Josh_ChoicesManager : MonoBehaviour
{
    [Header("Scene Names")]
    public string option1Scene;
    public string option2Scene;
    public string option3Scene;
    public string option4Scene;
    public string enterScene;
 
    [Header("UI")]
    public TextMeshProUGUI choicesText;
 
    void Start()
    {
        GameData.Load();
 
        // Only reset minigame flags if we've moved to a new city
        if (Josh_GameResultData.Instance != null)
        {
            if (GameData.CityIndex != GameData.LastChoicesCity)
            {
                GameData.LastChoicesCity = GameData.CityIndex;
                GameData.Save();
 
                Josh_GameResultData.Instance.scavengePlayed = false;
                Josh_GameResultData.Instance.gasPlayed      = false;
                Josh_GameResultData.Instance.lastCity       = GameData.CityIndex;
            }
        }
 
        UpdateUI();
    }
 
    void UpdateUI()
    {
        if (choicesText == null) return;
 
        string option1 = Josh_GameResultData.Instance != null && Josh_GameResultData.Instance.scavengePlayed
            ? "1.    SCAVENGE FOR FOOD (completed)"
            : "1.    SCAVENGE FOR FOOD";
 
        string option2 = Josh_GameResultData.Instance != null && Josh_GameResultData.Instance.gasPlayed
            ? "2.    FIND GAS (completed)"
            : "2.    FIND GAS";
 
        choicesText.text = option1 + "\n" +
                           option2 + "\n" +
                           "3.    INVENTORY\n" +
                           "4.    MAP\n\n" +
                           "PRESS ENTER TO CONTINUE TRAVELLING";
    }
 
    void Update()
    {
        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            if (Josh_GameResultData.Instance != null && Josh_GameResultData.Instance.scavengePlayed)
            {
                Debug.Log("Already scavenged this city!");
                return;
            }
            LoadScene(option1Scene);
        }
 
        if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            if (Josh_GameResultData.Instance != null && Josh_GameResultData.Instance.gasPlayed)
            {
                Debug.Log("Already found gas this city!");
                return;
            }
            LoadScene(option2Scene);
        }
 
        if (Keyboard.current.digit3Key.wasPressedThisFrame)
            LoadScene(option3Scene);
 
        if (Keyboard.current.digit4Key.wasPressedThisFrame)
            LoadScene(option4Scene);
 
        if (Keyboard.current.enterKey.wasPressedThisFrame)
            LoadScene(enterScene);
    }
 
    void LoadScene(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName)) return;
        SceneManager.LoadScene(sceneName);
    }
}
 