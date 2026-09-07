using UnityEngine;
using UnityEngine.InputSystem;

public class RM_AlignmentZoneController : MonoBehaviour
{
    public float riseSpeed = 3f;
    public float fallSpeed = 2f;
    public float boundTop = 3f;
    public float boundBottom = -3f;

    private bool _active = false;

    void Update()
    {
        if (!_active) return;

        Vector3 pos = transform.position;

        if (Keyboard.current.spaceKey.isPressed)
            pos.y += riseSpeed * Time.deltaTime;
        else
            pos.y -= fallSpeed * Time.deltaTime;

        pos.y = Mathf.Clamp(pos.y, boundBottom, boundTop);
        transform.position = pos;
    }

    public void StartGame() { _active = true; }
    public void StopGame() { _active = false; }
}