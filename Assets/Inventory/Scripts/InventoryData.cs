using UnityEngine;

[CreateAssetMenu(fileName = "InventoryData", menuName = "Game/Inventory Data")]
public class InventoryData : ScriptableObject
{
    public int food  = 50;
    public int gas   = 80;
    public int scrap = 20;
}