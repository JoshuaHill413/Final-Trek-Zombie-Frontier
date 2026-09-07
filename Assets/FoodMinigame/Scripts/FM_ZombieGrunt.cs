using UnityEngine;

public class FM_ZombieGrunt : MonoBehaviour
{
    public AudioClip gruntSound;
    public float minTime = 2f;
    public float maxTime = 6f;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        ScheduleNextGrunt();
    }

    void ScheduleNextGrunt()
    {
        float randomTime = Random.Range(minTime, maxTime);
        Invoke("PlayGrunt", randomTime);
    }

    void PlayGrunt()
    {
        audioSource.PlayOneShot(gruntSound);
        ScheduleNextGrunt();
    }
}