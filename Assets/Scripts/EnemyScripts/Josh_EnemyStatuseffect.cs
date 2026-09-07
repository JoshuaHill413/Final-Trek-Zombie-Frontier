using UnityEngine;

public class Josh_EnemyStatusEffect : MonoBehaviour
{
    public float slowMultiplier = 0.4f;
    public float slowDuration = 0.5f;
    private bool isSlowed = false;
    private Josh_EnemyMovement360 movement;

    void Awake()
    {
        movement = GetComponent<Josh_EnemyMovement360>();
    }

    public void ApplySlow()
    {
        if (isSlowed) return;

        isSlowed = true;
        movement.speed = movement.baseSpeed * slowMultiplier;
        Invoke("ResetSpeed", slowDuration);
    }

    public void ResetSpeed()
    {
        movement.speed = movement.baseSpeed;
        isSlowed = false;
    }
}