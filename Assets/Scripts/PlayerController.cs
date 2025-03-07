using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 8f;
    [SerializeField] private float dodgeForce = 10f;
    [SerializeField] private float dodgeCooldown = 1f;
    [SerializeField] private float gravity = -20f;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] private LayerMask groundMask;

    // Private variables
    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private bool canDodge = true;
    private float turnSmoothVelocity;
    private float turnSmoothTime = 0.1f;

    // Animation parameters (to be used later)
    private Animator animator;
    private readonly int isWalkingHash = Animator.StringToHash("isWalking");
    private readonly int isRunningHash = Animator.StringToHash("isRunning");
    private readonly int jumpHash = Animator.StringToHash("jump");
    private readonly int dodgeHash = Animator.StringToHash("dodge");

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Ground check
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // A small negative value instead of zero to ensure grounding
        }

        // Get input
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        // Move character
        if (direction.magnitude >= 0.1f)
        {
            // Calculate the angle to rotate towards
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            // Move in the facing direction
            bool isRunning = Input.GetKey(KeyCode.LeftShift);
            float currentSpeed = isRunning ? moveSpeed * 1.5f : moveSpeed;
            controller.Move(direction * currentSpeed * Time.deltaTime);

            // Animation
            if (animator != null)
            {
                animator.SetBool(isWalkingHash, !isRunning);
                animator.SetBool(isRunningHash, isRunning);
            }
        }
        else
        {
            // Reset animation when not moving
            if (animator != null)
            {
                animator.SetBool(isWalkingHash, false);
                animator.SetBool(isRunningHash, false);
            }
        }

        // Jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);

            // Animation
            if (animator != null)
            {
                animator.SetTrigger(jumpHash);
            }
        }

        // Dodge
        if (Input.GetKeyDown(KeyCode.LeftControl) && canDodge)
        {
            StartCoroutine(Dodge());
        }

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    IEnumerator Dodge()
    {
        canDodge = false;

        // Get dodge direction
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        // If no direction input, dodge forward
        if (direction.magnitude < 0.1f)
        {
            direction = transform.forward;
        }

        // Apply dodge force
        Vector3 dodgeVelocity = direction * dodgeForce;
        controller.Move(dodgeVelocity * Time.deltaTime);

        // Animation
        if (animator != null)
        {
            animator.SetTrigger(dodgeHash);
        }

        // Cooldown
        yield return new WaitForSeconds(dodgeCooldown);
        canDodge = true;
    }
}