using UnityEngine;
using UnityEngine.InputSystem;

// Handles player movement, jumping, and combat inputs.
// Requires PlayerInput component set to "Send Messages".
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 8f;
    public float jumpForce = 15f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public LayerMask groundLayer;
    public float groundCheckRadius = 0.2f;

    [Header("Graphics")]
    public Transform spriteTransform; // Drag your player's sprite object here

    private Rigidbody2D rb;
    private float horizontalMove = 0f;
    private bool isGrounded;
    private bool isFacingRight = true; // Tracks sprite direction

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation; // Prevents tumbling
    }

    private void FixedUpdate()
    {
        // Physics-based updates
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        rb.linearVelocity = new Vector2(horizontalMove * moveSpeed, rb.linearVelocity.y);

        FlipSprite();
    }

    // --- Input System Callbacks ---
    // Function names MUST match the Action names in your Input Action Asset.

    public void OnMove(InputValue value)
    {
        Vector2 moveInput = value.Get<Vector2>();
        horizontalMove = moveInput.x; // We only care about horizontal movement
    }

    public void OnJump(InputValue value)
    {
        if (value.isPressed && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    // Flips the sprite based on movement direction
    private void FlipSprite()
    {
        if (horizontalMove < 0 && isFacingRight)
        {
            // Face left
            isFacingRight = false;
            spriteTransform.localScale = new Vector3(-Mathf.Abs(spriteTransform.localScale.x), spriteTransform.localScale.y, spriteTransform.localScale.z);
        }
        else if (horizontalMove > 0 && !isFacingRight)
        {
            // Face right
            isFacingRight = true;
            spriteTransform.localScale = new Vector3(Mathf.Abs(spriteTransform.localScale.x), spriteTransform.localScale.y, spriteTransform.localScale.z);
        }
    }


    public void OnKick(InputValue value)
    {
        if (value.isPressed)
        {
            Debug.Log("PERFORM KICK!");
            // TODO: Kick attack logic
        }
    }

    public void OnUppercut(InputValue value)
    {
        if (value.isPressed)
        {
            Debug.Log("PERFORM HEAVY UPPERCUT!");
            // TODO: Uppercut attack logic
        }
    }

    public void OnDodgeRoll(InputValue value)
    {
        if (value.isPressed)
        {
            Debug.Log("PERFORM DODGE-ROLL!");
            // TODO: Dodge roll logic
        }
    }

    // Draws the green ground-check circle in the Scene view
    private void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}