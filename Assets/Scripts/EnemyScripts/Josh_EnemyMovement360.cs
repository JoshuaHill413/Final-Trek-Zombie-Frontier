using UnityEngine;

public class Josh_EnemyMovement360 : MonoBehaviour
{
    public Transform target;
    public float speed = 2f;
    public float baseSpeed = 2f;
    private Josh_EnemyStatusEffect statusEffect;
    private bool reachedCar = false;
    private Animator animator;

    void Start()
    {
        statusEffect = GetComponent<Josh_EnemyStatusEffect>();
        animator = GetComponent<Animator>();

        if (target == null)
        {
            GameObject car = GameObject.FindWithTag("Car");
            if (car != null) target = car.transform;
        }
    }

    void Update()
{
    if (target == null || reachedCar) return;

    Vector2 direction = (target.position - transform.position).normalized;
    transform.position += (Vector3)(direction * speed * Time.deltaTime);

    // Switch between left and right animation based on horizontal direction
    if (animator != null)
    {
        animator.SetFloat("MoveX", direction.x);
    }
}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Car") && !reachedCar)
        {
            reachedCar = true;
            Josh_CarHealth carHealth = FindAnyObjectByType<Josh_CarHealth>();
            if (carHealth != null) carHealth.TakeDamage(1);
            gameObject.SetActive(false);
        }
    }
}