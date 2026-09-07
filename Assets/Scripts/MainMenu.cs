using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// MainMenu.cs
// Attach this script to any persistent GameObject in your Main Menu scene
// (e.g. a "MenuManager" GameObject).
//
// In the Inspector, drag your Button components into the two exposed fields,
// and set gameSceneName to the exact Build Settings name of your gameplay scene.

public class MainMenu : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button quitButton;

    [Header("Scene Settings")]
    [Tooltip("The exact scene name as listed in File > Build Settings")]
    [SerializeField] private string gameSceneName = "Movement Scene";

    private void Start()
    {
        newGameButton.onClick.AddListener(OnNewGamePressed);
        quitButton.onClick.AddListener(OnQuitPressed);
    }

    private void OnDestroy()
    {
        newGameButton.onClick.RemoveListener(OnNewGamePressed);
        quitButton.onClick.RemoveListener(OnQuitPressed);
    }

    private void OnNewGamePressed()
    {
        // Optional: play click SFX / trigger transition here before loading
        SceneManager.LoadScene(gameSceneName);
    }

    private void OnQuitPressed()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;   // stops Play Mode in editor
#else
        Application.Quit();                                 // closes the built application
#endif
    }
}
