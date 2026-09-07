using UnityEngine;

/// <summary>
/// Attach to the same GameObject as TrailScroller.
/// Fades audio volume based on car speed and mutes on breakdown.
/// </summary>
public class AudioPauser : MonoBehaviour
{
    [Header("Audio Sources")]
    [Tooltip("All audio sources to control (engine, music, ambience etc.)")]
    public AudioSource[] audioSources;

    [Header("Settings")]
    [Tooltip("How fast the volume fades in/out (higher = faster).")]
    public float fadeSpeed = 2f;

    [Tooltip("Minimum volume when car is fully stopped (0 = silent).")]
    public float minVolume = 0f;

    [Tooltip("Maximum volume when car is at full speed.")]
    public float maxVolume = 1f;

    private TrailScroller _scroller;
    private bool _breakdownActive = false;
    private float[] _targetVolumes;

    void Start()
    {
        _scroller = GetComponent<TrailScroller>();

        // Store target volumes per source
        _targetVolumes = new float[audioSources.Length];
        for (int i = 0; i < audioSources.Length; i++)
            _targetVolumes[i] = audioSources[i] != null ? audioSources[i].volume : 1f;
    }

    void Update()
    {
        if (_scroller == null) return;

        float speedFactor = Mathf.Clamp01(_scroller.GetSpeedFactor());

        for (int i = 0; i < audioSources.Length; i++)
        {
            if (audioSources[i] == null) continue;

            float targetVolume;

            if (_breakdownActive)
            {
                // Fade out completely on breakdown
                targetVolume = 0f;
            }
            else
            {
                // Fade volume based on speed
                targetVolume = Mathf.Lerp(minVolume, maxVolume, speedFactor) * _targetVolumes[i];
            }

            audioSources[i].volume = Mathf.MoveTowards(
                audioSources[i].volume,
                targetVolume,
                fadeSpeed * Time.deltaTime
            );
        }
    }

    /// <summary>
    /// Call this when a breakdown starts to mute all audio.
    /// </summary>
    public void OnBreakdown()
    {
        _breakdownActive = true;
    }

    /// <summary>
    /// Call this when returning from repair to restore audio.
    /// </summary>
    public void OnRepaired()
    {
        _breakdownActive = false;
    }
}