using UnityEngine;
using TMPro;

public class LoseScreenUI : MonoBehaviour
{
    [Header("UI Text References")]
    public TextMeshProUGUI promptText;

    void Start()
    {
        if (promptText != null)
            promptText.text = "CLICK \"R\" TO RESTART THE GAME";
    }
}
