using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Movement Parameters
    public float speed = 5f;
    public float rotationSpeed = 10f;
    public float jumpForce = 7f;
    public float fallMultiplier = 2.5f; // Added for faster falling
    public float dashForce = 10f;      // Added dash force
    public float dashCooldown = 1f;    // Dash cooldown in seconds

    private Rigidbody rb;
    private Animator animator;
    private bool isGrounded;
    private bool canDash = true;       // Dash availability flag
    private Vector3 dashDirection;     // Stores dash direction

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        HandleMovement();
        HandleFalling(); // Added faster falling physics
    }

    void Update()
    {
        HandleJump();
        HandleDash(); // Added dash input handling
    }

    void HandleMovement()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        Vector3 movement = new Vector3(moveX, 0, moveZ).normalized;

        if (movement.magnitude > 0)
        {
            // Store movement direction for potential dash
            dashDirection = movement;

            rb.linearVelocity = new Vector3(movement.x * speed, rb.linearVelocity.y, movement.z * speed);
            
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            animator.SetBool("isRunning", true);
        }
        else
        {
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
            animator.SetBool("isRunning", false);
        }
    }

    void HandleFalling()
    {
        // Apply additional gravity when falling
        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
    }

    void HandleDash()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            // Apply dash force in the movement direction
            rb.AddForce(dashDirection * dashForce, ForceMode.Impulse);
            canDash = false;
            StartCoroutine(ResetDash());
        }
    }

    System.Collections.IEnumerator ResetDash()
    {
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            animator.SetTrigger("Jump");
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}