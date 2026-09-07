using UnityEngine;

public class TestWin : MonoBehaviour
{
    void Start()
    {
        GameData.CityIndex = 7;
        GameData.ScrollOffset = 1500f;
        GameData.Save();
    }
}