using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;
 
public class RM_GameManager : MonoBehaviour
{
    [Header("References")]
    public Transform gear;
    public Transform alignmentZone;
    public Slider progressSlider;
    public Slider riskSlider;
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI instructionText;
 
    [Header("Settings")]
    public float winTime = 20f;
    public float loseTime = 4f;
 
    [Header("Scenes")]
    public string movementScene = "Movement";
    public string loseScene     = "LoseScreen";
 
    [Header("Audio")]
    public AudioSource musicSource;
    public AudioSource sfxSource;
    public AudioClip winSound;
    public AudioClip loseSound;
    public AudioClip backgroundMusic;
 
    private float _alignedTime    = 0f;
    private float _misalignedTime = 0f;
    private bool _gameActive      = false;
    private bool _gameStarted     = false;
    private bool _gameOver        = false;
    private bool _isWin           = false;
 
    private SpriteRenderer _zoneSprite;
    private RM_GearController _gearController;
    private RM_AlignmentZoneController _zoneController;
    private RM_BackgroundShake _backgroundShake;
 
    void Start()
    {
        if (resultText != null)
            resultText.gameObject.SetActive(false);
 
        _zoneSprite       = alignmentZone.GetComponent<SpriteRenderer>();
        _gearController   = gear.GetComponent<RM_GearController>();
        _zoneController   = alignmentZone.GetComponent<RM_AlignmentZoneController>();
        _backgroundShake  = FindAnyObjectByType<RM_BackgroundShake>();

        _gearController.StopGame();
        _zoneController.StopGame();
        _backgroundShake.StopGame();
    }
 
    void Update()
    {
        if (!_gameStarted)
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
                StartGame();
            return;
        }
 
        if (_gameOver)
        {
            // Press Enter to continue after result is shown
            if (Keyboard.current.enterKey.wasPressedThisFrame)
            {
                if (_isWin)
                    SceneManager.LoadScene(movementScene);
                else
                    SceneManager.LoadScene(loseScene);
            }
            return;
        }
 
        if (!_gameActive) return;
 
        bool aligned = IsAligned();
 
        if (aligned)
        {
            _alignedTime += Time.deltaTime;
        }
        else
        {
            _misalignedTime += Time.deltaTime;
            _alignedTime = Mathf.Max(0f, _alignedTime - Time.deltaTime * 0.5f);
        }
 
        if (progressSlider != null)
            progressSlider.value = _alignedTime / winTime;
 
        if (riskSlider != null)
            riskSlider.value = _misalignedTime / loseTime;
 
        if (_alignedTime >= winTime)
        {
            _gameActive = false;
            _gameOver   = true;
            _isWin      = true;
            ShowResult("CAR FIXED!\nPRESS ENTER TO CONTINUE");
            PlaySound(winSound);
            StopAll();
        }
        else if (_misalignedTime >= loseTime)
        {
            _gameActive = false;
            _gameOver   = true;
            _isWin      = false;
            ShowResult("CAR IS TOTALED!\nPRESS ENTER TO CONTINUE");
            PlaySound(loseSound);
            StopAll();
        }
    }
 
    void StartGame()
    {
        _gameStarted = true;
        _gameActive  = true;
 
        if (instructionText != null)
            instructionText.gameObject.SetActive(false);
 
        _gearController.StartGame();
        _zoneController.StartGame();
        _backgroundShake.StartGame();
 
        PlayBackgroundMusic();
    }
 
    void StopAll()
    {
        if (_gearController  != null) _gearController.StopGame();
        if (_zoneController  != null) _zoneController.StopGame();
        if (_backgroundShake != null) _backgroundShake.StopGame();
        StopBackgroundMusic();
    }
 
    void PlaySound(AudioClip clip)
    {
        if (sfxSource != null && clip != null)
            sfxSource.PlayOneShot(clip);
    }
 
    void PlayBackgroundMusic()
    {
        if (musicSource != null && backgroundMusic != null)
        {
            musicSource.clip = backgroundMusic;
            musicSource.loop = true;
            musicSource.Play();
        }
    }
 
    void StopBackgroundMusic()
    {
        if (musicSource != null && musicSource.isPlaying)
            musicSource.Stop();
    }
 
    void ShowResult(string message)
    {
        if (resultText != null)
        {
            resultText.gameObject.SetActive(true);
            resultText.text = message;
        }
    }
 
    bool IsAligned()
    {
        float gearY     = gear.position.y;
        float zoneY     = alignmentZone.position.y;
        float halfHeight = _zoneSprite.bounds.extents.y;
        return gearY >= zoneY - halfHeight && gearY <= zoneY + halfHeight;
    }
}