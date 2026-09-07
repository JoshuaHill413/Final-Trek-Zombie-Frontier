using UnityEngine;

public class Josh_EnemyHealth : MonoBehaviour
{
    public int maxHealth = 15;
    public AudioClip deathSound;
    private int currentHealth;
    private Josh_EnemyStatusEffect statusEffect;

    void OnEnable()
    {
        currentHealth = maxHealth;
    }

    void Awake()
    {
        statusEffect = GetComponent<Josh_EnemyStatusEffect>();
    }

    public void TakeHit()
    {
        statusEffect.ApplySlow();
        currentHealth--;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (deathSound != null)
        {
            AudioSource.PlayClipAtPoint(deathSound, transform.position, 2f);
        }
        gameObject.SetActive(false);
    }
}