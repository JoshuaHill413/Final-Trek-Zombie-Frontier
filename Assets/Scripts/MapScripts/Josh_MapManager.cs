using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
 
public class Josh_MapManager : MonoBehaviour
{
    [System.Serializable]
    public class CityData
    {
        public string cityName;
        public Transform cityPoint;
        public GameObject[] lines;
        public GameObject staticIcon;
    }
 
    [Header("Cities")]
    public CityData[] cities;
    public int currentCity = 0;
 
    [Header("Blinking Icon")]
    public GameObject redIcon;
    public GameObject yellowIcon;
    public float blinkSpeed = 0.5f;
 
    [Header("Scenes")]
    public string returnScene;
 
    void Start()
    {
        // Sync map with GameData so it always reflects the correct city
        GameData.Load();
        currentCity = Mathf.Clamp(GameData.CityIndex, 0, cities.Length - 1);
 
        Debug.Log("MapManager started at city: " + currentCity);
        UpdateMap();
        StartCoroutine(BlinkLoop());
    }
 
    void Update()
    {
        
 
        if (Keyboard.current.enterKey.wasPressedThisFrame)
        {
            if (!string.IsNullOrEmpty(returnScene))
                SceneManager.LoadScene(returnScene);
        }
    }
 
    public void UpdateMap()
    {
        for (int i = 0; i < cities.Length; i++)
        {
            bool visited   = i < currentCity;
            bool isCurrent = i == currentCity;
 
            if (cities[i].staticIcon != null)
                cities[i].staticIcon.SetActive(visited);
 
            foreach (GameObject line in cities[i].lines)
                if (line != null) line.SetActive(visited || isCurrent);
        }
 
        if (cities[currentCity].staticIcon != null)
        {
            Vector3 iconPos = new Vector3(
                cities[currentCity].staticIcon.transform.position.x,
                cities[currentCity].staticIcon.transform.position.y,
                -1f
            );
            redIcon.transform.position    = iconPos;
            yellowIcon.transform.position = iconPos;
        }
    }
 
    IEnumerator BlinkLoop()
    {
        while (true)
        {
            redIcon.SetActive(true);
            yellowIcon.SetActive(false);
            yield return new WaitForSeconds(blinkSpeed);
 
            redIcon.SetActive(false);
            yellowIcon.SetActive(true);
            yield return new WaitForSeconds(blinkSpeed);
        }
    }
 
    public void SetCity(int newCity)
    {
        currentCity = Mathf.Clamp(newCity, 0, cities.Length - 1);
        UpdateMap();
 
        if (Josh_GameResultData.Instance != null)
            Josh_GameResultData.Instance.ResetForNewCity(currentCity);
    }
}
