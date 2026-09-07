using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

[System.Serializable]
public class SlideData
{
    public Sprite image;
    [TextArea(2, 5)]
    public string caption;
}

public class Josh_StorySlides : MonoBehaviour
{
    [Header("Slides")]
    public SlideData[] slides;

    [Header("UI")]
    public Image slideImage;
    public TextMeshProUGUI slideCaption;
    public Button nextButton;
    public TextMeshProUGUI buttonText;

    [Header("Scenes")]
    public string nextScene;

    private int currentSlide = 0;

    void Start()
    {
        if (slides.Length == 0)
        {
            Debug.LogWarning("No slides assigned!");
            return;
        }

        nextButton.onClick.AddListener(OnNextClicked);
        ShowSlide(0);
    }

    void ShowSlide(int index)
    {
        SlideData data = slides[index];

        slideImage.sprite = data.image;

        if (slideCaption != null)
            slideCaption.text = data.caption;

        buttonText.text = (index == slides.Length - 1) ? "Continue" : "Next";
    }

    void OnNextClicked()
    {
        currentSlide++;

        if (currentSlide >= slides.Length)
        {
            if (!string.IsNullOrEmpty(nextScene))
                SceneManager.LoadScene(nextScene);
            else
                Debug.LogWarning("No next scene assigned!");
        }
        else
        {
            ShowSlide(currentSlide);
        }
    }
}