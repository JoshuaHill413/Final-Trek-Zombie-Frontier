using UnityEngine;

public class Josh_CarHealth : MonoBehaviour
{
    public int maxHealth = 1;
    private int currentHealth;
    private Josh_GasMinigame gameMinigame;

    void Start()
    {
        currentHealth = maxHealth;
        gameMinigame = GetComponent<Josh_GasMinigame>();
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            PlayerDeath();
        }
    }

    void PlayerDeath()
    {
        gameMinigame.EndGame(false);
    }
}