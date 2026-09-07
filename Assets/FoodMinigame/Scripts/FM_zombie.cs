using UnityEngine;

public class FM_Zombie : MonoBehaviour
{
    public float speed = 2f;
    public float loseY = -4.5f;

    private FM_GameManager gameManager;

    void Start()
    {
        // 🔥 automatically find GameManager in the scene
        gameManager = FindAnyObjectByType<FM_GameManager>();
    }

    void Update()
    {
        transform.position += Vector3.down * speed * Time.deltaTime;

        // 💀 LOSE CONDITION
        if (transform.position.y < loseY)
        {
            if (gameManager != null)
            {
                gameManager.LoseGame();
            }

            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            if (gameManager != null)
            {
                gameManager.ZombieKilled();
            }

            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}