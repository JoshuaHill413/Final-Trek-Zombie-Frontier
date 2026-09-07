using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class CityRandomEvent : MonoBehaviour
{
    [Header("Scenes")]
    public string loseSceneName   = "LoseScreen";
    public string repairSceneName = "Car";

    [Header("Event Popup UI")]
    public GameObject eventPanel;
    public TextMeshProUGUI eventMessageText;
    public float displayDuration = 3f;

    public System.Action OnEventFinished;

    public void TriggerEvent()
    {
        StartCoroutine(RunEvent());
    }

    IEnumerator RunEvent()
    {
        // Load latest values before modifying
        GameData.Load();

        float roll = Random.value * 100f;
        string message;
        bool isDeath = false;

        if (roll < 20f)
        {
            message = "You slip into the city unnoticed...\nAll is quiet.";
        }
        else if (roll < 32f)
        {
            int amount = WeightedLow(5, 15);
            GameData.Food += amount;
            message = $"You find an abandoned grocery store!\n+{amount} Food";
        }
        else if (roll < 44f)
        {
            int amount = WeightedLow(5, 15);
            GameData.Food = Mathf.Max(0, GameData.Food - amount);
            message = $"Rats got into your supplies...\n-{amount} Food";
        }
        else if (roll < 56f)
        {
            int amount = WeightedLow(5, 50);
            GameData.Gas += amount;
            message = $"You siphon gas from abandoned cars!\n+{amount} Gallons";
        }
        else if (roll < 68f)
        {
            int amount = WeightedLow(5, 25);
            GameData.Gas = Mathf.Max(0, GameData.Gas - amount);
            message = $"Your tank has a slow leak...\n-{amount} Gallons";
        }
        else if (roll < 74f)
        {
            GameData.Gas *= 2;
            message = $"You find a fuel depot!\nGas supply DOUBLED to {GameData.Gas}!";
        }
        else if (roll < 80f)
        {
            GameData.Gas = Mathf.Max(0, GameData.Gas / 2);
            message = $"Your fuel line ruptures...\nGas reduced to {GameData.Gas}.";
        }
        else if (roll < 86f)
        {
            GameData.Food *= 2;
            message = $"A survivor shares their cache!\nFood supply DOUBLED to {GameData.Food}!";
        }
        else if (roll < 92f)
        {
            GameData.Food = Mathf.Max(0, GameData.Food / 2);
            message = $"Your food stores are contaminated...\nFood reduced to {GameData.Food}.";
        }
        else if (roll < 97f)
        {
            GameData.HasSuperPart = true;
            message = "You find a legendary engine part!\nYour car will never break down again!";
        }
        else
        {
            isDeath = true;
            message = "ZOMBIES SWARM YOUR CAR!\nYou didn't make it...";
        }

        // Save immediately after modifying
        if (!isDeath)
            GameData.Save();

        yield return StartCoroutine(ShowMessage(message, displayDuration));

        if (isDeath)
            SceneManager.LoadScene(loseSceneName);
        else
            OnEventFinished?.Invoke();
    }

    IEnumerator ShowMessage(string message, float duration)
    {
        if (eventPanel != null)
        {
            eventPanel.SetActive(true);
            if (eventMessageText != null)
                eventMessageText.text = message;
        }

        yield return new WaitForSeconds(duration);

        if (eventPanel != null)
            eventPanel.SetActive(false);
    }

    int WeightedLow(int min, int max)
    {
        float t = Mathf.Sqrt(Random.value);
        return Mathf.RoundToInt(Mathf.Lerp(min, max, 1f - t));
    }
}