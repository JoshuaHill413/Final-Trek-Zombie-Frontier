using UnityEngine;
 
public class CarJostle : MonoBehaviour
{
    [Header("Bounce Settings")]
    public float bounceAmplitude = 0.04f;
    public float bounceFrequency = 8f;
 
    [Header("Tilt Settings")]
    public float tiltAmplitude = 0.6f;
    public float tiltFrequency = 6f;
 
    [Header("Random Bumps")]
    public bool  enableRandomBumps = true;
    public float bumpStrength      = 0.03f;
    public float bumpInterval      = 0.4f;
 
    [Header("Speed Scaling")]
    [Tooltip("Set by TrailScroller each frame (0 = stopped, 1 = full cruise). " +
             "You can also keyframe this manually.")]
    [Range(0f, 1f)]
    public float speedFactor = 0f;
 
    [Tooltip("How quickly jostle intensity ramps up/down when speedFactor changes. " +
             "Higher = snappier response.")]
    public float smoothing = 4f;
 
    // Private
    private Vector3    _originLocalPos;
    private Quaternion _originLocalRot;
 
    private float _bumpOffsetY;
    private float _bumpOffsetRoll;
    private float _bumpDecay  = 8f;
    private float _bumpTimer;
    private float _timeOffset;
 
    private float _smoothedFactor; // lerped version of speedFactor
 
    void Start()
    {
        _originLocalPos = transform.localPosition;
        _originLocalRot = transform.localRotation;
        _timeOffset     = Random.Range(0f, Mathf.PI * 2f);
        _smoothedFactor = speedFactor;
    }
 
    void Update()
    {
        // Smooth the speed factor so jostle doesn't snap on/off
        _smoothedFactor = Mathf.Lerp(_smoothedFactor, speedFactor, Time.deltaTime * smoothing);
 
        float t = Time.time + _timeOffset;
        float s = _smoothedFactor;
 
        // 1. Vertical bounce
        float bounceY = Mathf.Sin(t * bounceFrequency * Mathf.PI * 2f) * bounceAmplitude * s;
 
        // 2. Tilt
        float roll  = Mathf.Sin(t * tiltFrequency * Mathf.PI * 2f)              * tiltAmplitude * s;
        float pitch = Mathf.Sin(t * tiltFrequency * Mathf.PI * 2f * 0.7f + 1.2f) * tiltAmplitude * 0.4f * s;
 
        // 3. Random bumps
        if (enableRandomBumps)
        {
            _bumpTimer -= Time.deltaTime;
            if (_bumpTimer <= 0f)
            {
                _bumpOffsetY    = Random.Range(-1f, 1f) * bumpStrength * s;
                _bumpOffsetRoll = Random.Range(-1f, 1f) * tiltAmplitude * 0.5f * s;
                _bumpTimer      = bumpInterval + Random.Range(-0.1f, 0.1f);
            }
 
            _bumpOffsetY    = Mathf.Lerp(_bumpOffsetY,    0f, Time.deltaTime * _bumpDecay);
            _bumpOffsetRoll = Mathf.Lerp(_bumpOffsetRoll, 0f, Time.deltaTime * _bumpDecay);
        }
 
        // 4. Apply
        transform.localPosition = _originLocalPos + new Vector3(0f, bounceY + _bumpOffsetY, 0f);
        Quaternion jostle = Quaternion.Euler(pitch, 0f, roll + _bumpOffsetRoll);
        transform.localRotation = _originLocalRot * jostle;
    }
 
    public void RefreshOrigin()
    {
        _originLocalPos = transform.localPosition;
        _originLocalRot = transform.localRotation;
    }
 
    public void TriggerBump(float strengthMultiplier = 1f)
    {
        _bumpOffsetY    = Random.Range(0.5f, 1f) * bumpStrength * strengthMultiplier;
        _bumpOffsetRoll = Random.Range(-1f,  1f) * tiltAmplitude * 0.8f * strengthMultiplier;
    }
}