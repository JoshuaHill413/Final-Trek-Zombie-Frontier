using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
 
public class InventoryPause : MonoBehaviour
{
    [Header("Panel")]
    public GameObject inventoryPanel;
 
    [Header("Buttons")]
    public Button openButton;
 
    private InventoryUI _inventoryUI;
    private bool _isOpen = false;
    private int _lastCityIndex = -1;
 
    void Start()
    {
        if (inventoryPanel != null)
        {
            inventoryPanel.SetActive(false);
            _inventoryUI = inventoryPanel.GetComponent<InventoryUI>();
        }
 
        if (openButton != null)
            openButton.onClick.AddListener(OpenInventory);
 
        _lastCityIndex = GameData.CityIndex;
    }
 
    void OnDestroy()
    {
        if (openButton != null)
            openButton.onClick.RemoveAllListeners();
    }
 
    void Update()
    {
        if (_isOpen && Keyboard.current != null && Keyboard.current.enterKey.wasPressedThisFrame)
            CloseInventory();
 
        // Close instantly if a new city is reached
        if (_isOpen && GameData.CityIndex != _lastCityIndex)
        {
            _lastCityIndex = GameData.CityIndex;
            CloseInventory();
        }
    }
 
    void OpenInventory()
    {
        if (_isOpen) return;
        _isOpen = true;
 
        if (_inventoryUI != null) _inventoryUI.UpdateUI();
        if (inventoryPanel != null) inventoryPanel.SetActive(true);
    }
 
    void CloseInventory()
    {
        if (!_isOpen) return;
        _isOpen = false;
 
        if (inventoryPanel != null) inventoryPanel.SetActive(false);
    }
}