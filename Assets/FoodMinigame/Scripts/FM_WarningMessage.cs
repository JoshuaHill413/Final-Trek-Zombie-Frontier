using UnityEngine;
using TMPro;
using System.Collections;

public class FM_WarningMessage : MonoBehaviour
{
    public float displayTime = 2f;
    private TextMeshProUGUI warningText;

    void Start()
    {
        warningText = GetComponent<TextMeshProUGUI>();
        warningText.text = "Don't let the zombies get past you!";
        StartCoroutine(HideMessage());
    }

    IEnumerator HideMessage()
    {
        yield return new WaitForSeconds(displayTime);
        gameObject.SetActive(false);
    }
}