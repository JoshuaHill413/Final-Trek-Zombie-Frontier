using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class TrailScroller : MonoBehaviour
{
    public static TrailScroller Instance { get; private set; }

    [Header("City Stop Distances (cumulative scroll units)")]
    public float[] cityStopOffsets = new float[]
    {
        97.91961f,
        254.9173f,
        446.1996f,
        671.41f,
        880.7896f,
        1200.36f,
        1500f,
        1676.83f
    };

    [Header("Speed")]
    public float cruiseSpeed          = 8f;
    public float accelerationTime     = 1.5f;
    public float decelerationDistance = 3f;
    public float stopThreshold        = 0.05f;

    [Header("Timing")]
    public float pauseAtCityDuration  = 1f;

    [Header("Scene Transition")]
    public string choicesSceneName   = "Choices";
    public string repairSceneName    = "Car";
    public string loseSceneName      = "LoseScreen";
    public string winSceneName       = "WinScreen";

    [Header("Car Breakdown")]
    public float breakdownChance      = 0.5f;
    public float breakdownMinFraction = 0.3f;
    public float breakdownMaxFraction = 0.7f;
    public int   breakdownCooldown    = 2;

    [Header("Breakdown Popup UI")]
    public GameObject breakdownPanel;
    public TMPro.TextMeshProUGUI breakdownMessageText;
    public float breakdownDisplayDuration = 3f;

    [Header("Resource Depletion")]
    [Tooltip("Gas consumed per second of driving.")]
    public float gasPerSecond  = 1f;
    [Tooltip("Food consumed per second of driving.")]
    public float foodPerSecond = 0.5f;

    [Header("Out of Resource UI")]
    public GameObject outOfResourcePanel;
    public TMPro.TextMeshProUGUI outOfResourceText;
    public float outOfResourceDisplayDuration = 3f;

    [Header("Car Components")]
    public CarJostle carJostle;
    public TireRotation[] tireRotations;

    // Events
    public System.Action<int> OnCityReached;
    public System.Action<int> OnLegStarted;

    // Private state
    private enum Phase { Idle, Accelerating, Cruising, Decelerating, Stopped }
    private Phase _phase = Phase.Idle;

    private int   _nextCityIndex       = 0;
    private float _currentSpeed        = 0f;
    private float _accelTimer          = 0f;
    private float _scrolled            = 0f;
    private bool  _journeyStarted      = false;
    private bool  _breakdownTriggered  = false;
    private float _breakdownOffset     = -1f;
    private bool  _depletionTriggered  = false;

    // Fractional accumulators so we don't lose decimals on int resources
    private float _gasAccum   = 0f;
    private float _foodAccum  = 0f;
    private float _saveTimer  = 0f;
    private const float SaveInterval = 1f; // save every 1 second

    private Vector3 _startPos;
    private CityRandomEvent _randomEvent;
    private AudioPauser _audioPauser;

    // =========================================================================
    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        _startPos = transform.position;
        _randomEvent = GetComponent<CityRandomEvent>();
        _audioPauser  = GetComponent<AudioPauser>();
    }

    void Start()
    {
        if (breakdownPanel != null)       breakdownPanel.SetActive(false);
        if (outOfResourcePanel != null)   outOfResourcePanel.SetActive(false);

        GameData.Load();
        _nextCityIndex = GameData.CityIndex;
        _scrolled      = GameData.ScrollOffset;
        ApplyScroll();

        StartJourney();
    }

    void Update()
    {
        if (_phase == Phase.Stopped || _phase == Phase.Idle)
        {
            SyncCarToSpeed(0f);
            return;
        }

        // Deplete resources while driving
        if (_currentSpeed > 0f && !_depletionTriggered)
        {
            float dt = Time.deltaTime;

            _gasAccum  += gasPerSecond  * dt;
            _foodAccum += foodPerSecond * dt;

            int gasToRemove  = Mathf.FloorToInt(_gasAccum);
            int foodToRemove = Mathf.FloorToInt(_foodAccum);

            if (gasToRemove > 0)
            {
                GameData.Gas  = Mathf.Max(0, GameData.Gas  - gasToRemove);
                _gasAccum    -= gasToRemove;
            }

            if (foodToRemove > 0)
            {
                GameData.Food  = Mathf.Max(0, GameData.Food  - foodToRemove);
                _foodAccum    -= foodToRemove;
            }

            // Save every second so inventory reflects live values
            _saveTimer += dt;
            if (_saveTimer >= SaveInterval)
            {
                GameData.Save();
                _saveTimer = 0f;
            }

            // Out of gas — stranded!
            if (GameData.Gas <= 0)
            {
                _depletionTriggered = true;
                _phase = Phase.Stopped;
                SyncCarToSpeed(0f);
                StartCoroutine(OutOfResourceRoutine("You ran out of gas!\nYou're stranded..."));
                return;
            }

            // Out of food — starved!
            if (GameData.Food <= 0)
            {
                _depletionTriggered = true;
                _phase = Phase.Stopped;
                SyncCarToSpeed(0f);
                StartCoroutine(OutOfResourceRoutine("You ran out of food!\nYour crew starved..."));
                return;
            }
        }

        // Check for mid-drive breakdown
        if (!_breakdownTriggered && _breakdownOffset > 0 && _scrolled >= _breakdownOffset)
        {
            _breakdownTriggered  = true;
            GameData.LastBreakdownCity   = _nextCityIndex;
            _phase               = Phase.Stopped;
            SyncCarToSpeed(0f);
            StartCoroutine(BreakdownRoutine());
            return;
        }

        float targetOffset = cityStopOffsets[_nextCityIndex];
        float distToStop   = targetOffset - _scrolled;

        switch (_phase)
        {
            case Phase.Accelerating:
                _accelTimer  += Time.deltaTime;
                float accelT  = Mathf.Clamp01(_accelTimer / accelerationTime);
                _currentSpeed = Mathf.Lerp(0f, cruiseSpeed, accelT * accelT);
                if (accelT >= 1f)                       _phase = Phase.Cruising;
                if (distToStop <= decelerationDistance) _phase = Phase.Decelerating;
                break;

            case Phase.Cruising:
                _currentSpeed = cruiseSpeed;
                if (distToStop <= decelerationDistance) _phase = Phase.Decelerating;
                break;

            case Phase.Decelerating:
                float decelT  = Mathf.Clamp01(distToStop / decelerationDistance);
                _currentSpeed = Mathf.Lerp(0f, cruiseSpeed, decelT * decelT);
                if (_currentSpeed <= stopThreshold || distToStop <= 0f)
                {
                    _scrolled     = targetOffset;
                    _currentSpeed = 0f;
                    ApplyScroll();
                    SyncCarToSpeed(0f);
                    ArriveAtCity();
                    return;
                }
                break;
        }

        _scrolled += _currentSpeed * Time.deltaTime;
        ApplyScroll();
        SyncCarToSpeed(_currentSpeed);
    }

    // =========================================================================
    void ApplyScroll()
    {
        transform.position = new Vector3(
            _startPos.x + _scrolled,
            _startPos.y,
            _startPos.z
        );
    }

    void SyncCarToSpeed(float speed)
    {
        float t = Mathf.Clamp01(speed / cruiseSpeed);

        if (carJostle != null)
            carJostle.speedFactor = t;

        if (tireRotations != null)
            foreach (var tire in tireRotations)
                if (tire != null)
                    tire.rotationSpeed = t * tire.maxRotationSpeed;
    }

    // =========================================================================
    public void StartJourney()
    {
        if (_journeyStarted) return;
        _journeyStarted = true;
        BeginLeg();
    }

    void BeginLeg()
    {
        _accelTimer         = 0f;
        _currentSpeed       = 0f;
        _breakdownTriggered = false;
        _depletionTriggered = false;
        _gasAccum           = 0f;
        _foodAccum          = 0f;
        _phase              = Phase.Accelerating;

        bool cooledDown   = (_nextCityIndex - GameData.LastBreakdownCity) >= breakdownCooldown;
        bool canBreakdown = !GameData.HasSuperPart && _nextCityIndex > 0 && cooledDown;

        if (canBreakdown && Random.value < breakdownChance)
        {
            float legStart   = cityStopOffsets[_nextCityIndex - 1];
            float legEnd     = cityStopOffsets[_nextCityIndex];
            float legLength  = legEnd - legStart;
            float fraction   = Random.Range(breakdownMinFraction, breakdownMaxFraction);
            _breakdownOffset = legStart + legLength * fraction;
        }
        else
        {
            _breakdownOffset = -1f;
        }

        OnLegStarted?.Invoke(_nextCityIndex);
    }

    void ArriveAtCity()
    {
        _phase = Phase.Stopped;

        GameData.CityIndex    = _nextCityIndex + 1;
        GameData.ScrollOffset = _scrolled;
        GameData.Save();

        OnCityReached?.Invoke(_nextCityIndex);
        StartCoroutine(CityPauseRoutine());
    }

    IEnumerator CityPauseRoutine()
    {
        yield return new WaitForSeconds(pauseAtCityDuration);

        if (_randomEvent != null)
        {
            bool eventDone = false;
            _randomEvent.OnEventFinished = () => eventDone = true;
            _randomEvent.TriggerEvent();
            yield return new WaitUntil(() => eventDone);
        }

        // If this was the last city load the win screen
        if (_nextCityIndex >= cityStopOffsets.Length - 1)
            SceneManager.LoadScene(winSceneName);
        else
            SceneManager.LoadScene(choicesSceneName);
    }

    public float GetSpeedFactor()
    {
        return Mathf.Clamp01(_currentSpeed / cruiseSpeed);
    }

    IEnumerator BreakdownRoutine()
    {
        if (_audioPauser != null) _audioPauser.OnBreakdown();

        if (breakdownPanel != null)
        {
            breakdownPanel.SetActive(true);
            if (breakdownMessageText != null)
                breakdownMessageText.text = "Your car broke down.\nTry to repair it.";
        }

        yield return new WaitForSeconds(breakdownDisplayDuration);

        if (breakdownPanel != null)
            breakdownPanel.SetActive(false);

        // Save scroll AND current city index (do NOT increment — we haven't reached the city yet)
        GameData.ScrollOffset = _scrolled;
        GameData.CityIndex    = _nextCityIndex;
        GameData.Save();

        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(repairSceneName);
    }

    IEnumerator OutOfResourceRoutine(string message)
    {
        if (outOfResourcePanel != null)
        {
            outOfResourcePanel.SetActive(true);
            if (outOfResourceText != null)
                outOfResourceText.text = message;
        }

        GameData.Save();
        yield return new WaitForSeconds(outOfResourceDisplayDuration);

        if (outOfResourcePanel != null)
            outOfResourcePanel.SetActive(false);

        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(loseSceneName);
    }
}