using UnityEngine;
using UnityEngine.InputSystem;
 
public class FM_PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 5f;
 
    [Header("References")]
    [SerializeField] private Rigidbody2D rb2d;
    [SerializeField] private SpriteRenderer sr;
    private Animator animator;
 
    private Vector2 movement;
 
    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }
 
    private void Update()
    {
        movement.x = 0f;
        movement.y = 0f;
 
        if (Keyboard.current.leftArrowKey.isPressed || Keyboard.current.aKey.isPressed)
            movement.x = -1f;
        else if (Keyboard.current.rightArrowKey.isPressed || Keyboard.current.dKey.isPressed)
            movement.x = 1f;
 
        if (Keyboard.current.upArrowKey.isPressed || Keyboard.current.wKey.isPressed)
            movement.y = 1f;
        else if (Keyboard.current.downArrowKey.isPressed || Keyboard.current.sKey.isPressed)
            movement.y = -1f;
 
        // Play animations based on movement
        if (movement.x > 0)
        {
            animator.Play("PlayerRight");
            sr.flipX = false;
        }
        else if (movement.x < 0)
        {
            animator.Play("PlayerLeft");
            sr.flipX = true;
        }
        else if (movement.y != 0)
        {
            animator.Play("PlayerWalk");
        }
        else
        {
            animator.Play("PlayerIdle");
        }
    }
 
    private void FixedUpdate()
    {
        rb2d.MovePosition(rb2d.position + movement.normalized * speed * Time.fixedDeltaTime);
    }
}