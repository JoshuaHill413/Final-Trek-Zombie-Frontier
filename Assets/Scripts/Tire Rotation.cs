using UnityEngine;
 
public class TireRotation : MonoBehaviour
{
    [Tooltip("Rotation speed at full cruise. TrailScroller scales this down " +
             "to 0 when stopped and back up during acceleration.")]
    public float maxRotationSpeed = 300f;
 
    [Tooltip("Current rotation speed — set at runtime by TrailScroller. " +
             "You can also set it manually if not using TrailScroller.")]
    [HideInInspector]
    public float rotationSpeed = 0f;
 
    //public AudioClip   carDriveClip;
    //public AudioSource audioSource;
 
    void Start()
    {
        // audioSource      = GetComponent<AudioSource>();
        // audioSource.clip = carDriveClip;
        // audioSource.loop = true;
        // audioSource.Play();
    }
 
    void Update()
    {
        // Rotate at whatever speed TrailScroller has set
        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime, Space.Self);
 
        // Pitch the engine audio up with speed so it sounds like acceleration
        // if (audioSource != null && TrailScroller.Instance != null)
        // {
        //     float t = Mathf.Clamp01(rotationSpeed / Mathf.Max(maxRotationSpeed, 0.001f));
        //     audioSource.pitch  = Mathf.Lerp(0.5f, 1.2f, t);
        //     audioSource.volume = Mathf.Lerp(0.2f, 1.0f, t);
 
        //     if (!audioSource.isPlaying) audioSource.Play();
        // }
    }
}