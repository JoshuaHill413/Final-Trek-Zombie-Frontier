using UnityEngine;
using TMPro;
 
public class InventoryUI : MonoBehaviour
{
    [Header("Labels")]
    [SerializeField] private TextMeshProUGUI foodLabel;
    [SerializeField] private TextMeshProUGUI gasLabel;
 
    private void Start()
    {
        GameData.Load();
        UpdateUI();
    }
 
    private void OnEnable()
    {
        GameData.Load();
        UpdateUI();
    }
 
    public void UpdateUI()
    {
        if (foodLabel != null) foodLabel.text = $"FOOD:  {GameData.Food} LBS";
        if (gasLabel  != null) gasLabel.text  = $"GAS:   {GameData.Gas} GAL";
    }
 
    public void AddFood(int amount)    { GameData.Food = Mathf.Max(0, GameData.Food + amount); GameData.Save(); UpdateUI(); }
    public void AddGas(int amount)     { GameData.Gas  = Mathf.Max(0, GameData.Gas  + amount); GameData.Save(); UpdateUI(); }
    public void RemoveFood(int amount) { GameData.Food = Mathf.Max(0, GameData.Food - amount); GameData.Save(); UpdateUI(); }
    public void RemoveGas(int amount)  { GameData.Gas  = Mathf.Max(0, GameData.Gas  - amount); GameData.Save(); UpdateUI(); }
}
 





