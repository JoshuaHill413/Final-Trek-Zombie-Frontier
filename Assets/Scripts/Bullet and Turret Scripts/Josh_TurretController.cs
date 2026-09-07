using UnityEngine;
using UnityEngine.InputSystem;

public class Josh_TurretController : MonoBehaviour
{
    public float rotationSpeed = 20f;
    private Vector3 mousePosition;

    void Update()
    {
        RotateTurret();
    }

    void RotateTurret()
    {
        // New Input System uses Mouse.current instead of Input.mousePosition
        Vector2 rawMousePos = Mouse.current.position.ReadValue();
        mousePosition = Camera.main.ScreenToWorldPoint(rawMousePos);

        Vector2 direction = new Vector2(
            mousePosition.x - transform.position.x,
            mousePosition.y - transform.position.y
        );

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;

        Quaternion targetRotation = Quaternion.Euler(0f, 0f, angle);

        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }
}