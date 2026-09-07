using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
 
public class InventoryBack : MonoBehaviour
{
    [Header("Scene to return to")]
    public string previousScene = "Movement Scene";
 
    void Update()
    {
        if (Keyboard.current.enterKey.wasPressedThisFrame)
        {
            SceneManager.LoadScene(previousScene);
        }
    }
}
 