using UnityEngine;

public class RM_BackgroundShake : MonoBehaviour
{
    public float shakeIntensity = 0.05f;
    public float shakeSpeed = 20f;

    private Vector3 _originPos;
    private bool _active = false;

    void Start()
    {
        _originPos = transform.position;
    }

    void Update()
    {
        if (!_active) return;
        float offsetX = Mathf.Sin(Time.time * shakeSpeed) * shakeIntensity;
        transform.position = _originPos + new Vector3(offsetX, 0f, 0f);
    }

    public void StartGame() { _active = true; }
    public void StopGame()
    {
        _active = false;
        transform.position = _originPos;
    }
}