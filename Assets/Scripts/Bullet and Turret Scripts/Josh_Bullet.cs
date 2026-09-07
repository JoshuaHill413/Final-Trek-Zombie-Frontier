using UnityEngine;

public class Josh_Bullet : MonoBehaviour
{
    public float speed = 15f;
    public float lifetime = 1.5f;
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        CancelInvoke();
        Invoke("ReturnToPool", lifetime);
    }

    public void Launch(Vector2 direction)
    {
        rb.linearVelocity = direction.normalized * speed;
    }

   void OnTriggerEnter2D(Collider2D other)
{
    if (other.CompareTag("Enemy"))
    {
        Josh_EnemyHealth health = other.GetComponent<Josh_EnemyHealth>();
        if (health != null) health.TakeHit();

        Josh_BulletPooler.Instance.ReturnBullet(gameObject);
    }
}

    void ReturnToPool()
    {
        rb.linearVelocity = Vector2.zero;
        Josh_BulletPooler.Instance.ReturnBullet(gameObject);
    }
}