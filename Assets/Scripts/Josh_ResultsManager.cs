using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
 
public class Josh_ResultsManager : MonoBehaviour
{
    [Header("Icons")]
    public GameObject gasIcon;
    public GameObject foodIcon;
 
    [Header("Text")]
    public TextMeshProUGUI resultText;
 
    [Header("Result Settings")]
    public bool isWin;
    public int resourceAmount;
    public enum ResourceType { Gas, Food }
    public ResourceType resourceType;
    public string winHexColor  = "00FF00";
    public string loseHexColor = "FF0000";
 
    [Header("Next Scene")]
    public string nextScene;
 
    void Start()
    {
        if (Josh_GameResultData.Instance != null)
        {
            isWin          = Josh_GameResultData.Instance.isWin;
            resourceAmount = Josh_GameResultData.Instance.resourceAmount;
 
            switch (Josh_GameResultData.Instance.resourceType)
            {
                case "Gas":  resourceType = ResourceType.Gas;  break;
                case "Food": resourceType = ResourceType.Food; break;
            }
        }
 
        ApplyResultToGameData();
 
        gasIcon.SetActive(false);
        foodIcon.SetActive(false);
 
        switch (resourceType)
        {
            case ResourceType.Gas:  gasIcon.SetActive(true);  break;
            case ResourceType.Food: foodIcon.SetActive(true); break;
        }
 
        string sign     = isWin ? "+" : "-";
        string resource = resourceType.ToString();
        resultText.text = sign + resourceAmount + " " + resource;
 
        string hex = isWin ? winHexColor : loseHexColor;
        Color color;
        if (ColorUtility.TryParseHtmlString("#" + hex, out color))
            resultText.color = color;
    }
 
    void ApplyResultToGameData()
    {
        GameData.Load();
 
        if (isWin)
        {
            switch (resourceType)
            {
                case ResourceType.Gas:  GameData.Gas  += resourceAmount; break;
                case ResourceType.Food: GameData.Food += resourceAmount; break;
            }
        }
        else
        {
            switch (resourceType)
            {
                case ResourceType.Gas:  GameData.Gas  = Mathf.Max(0, GameData.Gas  - resourceAmount); break;
                case ResourceType.Food: GameData.Food = Mathf.Max(0, GameData.Food - resourceAmount); break;
            }
        }
 
        GameData.Save();
    }
 
    void Update()
    {
        if (Keyboard.current.enterKey.wasPressedThisFrame)
            if (!string.IsNullOrEmpty(nextScene))
                SceneManager.LoadScene(nextScene);
    }
}
 