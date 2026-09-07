using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
 
public class RestartGame : MonoBehaviour
{
    public string gameScene = "SampleScene";
 
    void Update()
    {
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            GameData.Reset();
            PlayerPrefs.DeleteAll();
            SceneManager.LoadScene(gameScene);
        }
    }
}
 