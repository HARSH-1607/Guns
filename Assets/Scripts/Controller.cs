using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 5.0f;
    public float runSpeed = 8.0f;
    public float jumpHeight = 1.5f;
    public float gravity = -9.81f;
    
    [Header("Mouse Settings")]
    public float mouseSensitivity = 100f;
    private float xRotation = 0f;

    [Header("Dash Settings")]
    public float dashSpeed = 20.0f;
    public float dashDuration = 0.25f;
    public float dashCooldown = 1.0f;

    [Header("Stamina Settings")]
    public float maxStamina = 100f;
    public float currentStamina;
    public float sprintStaminaCost = 20f;
    public float dashStaminaCost = 25f;
    public float staminaRegenRate = 15f;
    public float staminaRegenDelay = 2.0f;
    private float staminaRegenTimer;

    private CharacterController controller;
    private Transform playerCamera;
    private Vector3 velocity;
    private bool isGrounded;

    // Dash state variables
    private bool isDashing = false;
    private float dashTimer;
    private float dashCooldownTimer = 0f;
    private Vector3 dashDirection;

    // Input Action variables
    private PlayerInput playerInputActions;
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction lookAction;
    private InputAction sprintAction;

    // Reference to the PlayerHealth script
    private PlayerHealth playerHealth;

    private void Awake()
    {
        playerInputActions = new PlayerInput();
        moveAction = playerInputActions.OnFoot.Movement;
        jumpAction = playerInputActions.OnFoot.Jump;
        lookAction = playerInputActions.OnFoot.Look;
        sprintAction = playerInputActions.OnFoot.Sprint;
    }

    private void OnEnable()
    {
        playerInputActions.OnFoot.Enable();
    }
    private void OnDisable()
    {
        playerInputActions.OnFoot.Disable();
    }

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        playerCamera = Camera.main.transform;
        playerHealth = GetComponent<PlayerHealth>();

        if (playerHealth == null)
        {
            Debug.LogWarning("PlayerHealth component not found on this object! Stamina bar will not update.");
        }

        currentStamina = maxStamina;

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        HandleMouseLook();
        HandleDashing();
        HandleMovement();
        HandleJumping();
        ApplyGravity();
        HandleStamina(); 
    }

    private void HandleMouseLook()
    {
        Vector2 lookInput = lookAction.ReadValue<Vector2>();
        float mouseX = lookInput.x * (mouseSensitivity * 0.1f) * Time.deltaTime;
        float mouseY = lookInput.y * (mouseSensitivity * 0.1f) * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    private void HandleMovement()
    {
        if (isDashing) return;

        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        Vector2 moveInput = moveAction.ReadValue<Vector2>();
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        
        bool isSprinting = sprintAction.IsPressed() && (moveInput.magnitude > 0.1f) && currentStamina > 0;
        
        float speed = walkSpeed;
        if (isSprinting)
        {
            speed = runSpeed;
            UseStamina(sprintStaminaCost * Time.deltaTime);
        }

        controller.Move(move * speed * Time.deltaTime);
    }

    private void HandleJumping()
    {
        if (jumpAction.WasPressedThisFrame() && isGrounded && !isDashing)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    private void HandleDashing()
    {
        if (dashCooldownTimer > 0)
        {
            dashCooldownTimer -= Time.deltaTime;
        }

        // MODIFIED: Changed KeyCode.V to KeyCode.LeftControl
        if (Input.GetKeyDown(KeyCode.LeftControl) && dashCooldownTimer <= 0 && currentStamina >= dashStaminaCost)
        {
            UseStamina(dashStaminaCost);
            isDashing = true;
            dashTimer = dashDuration;
            dashCooldownTimer = dashCooldown;

            Vector2 moveInput = moveAction.ReadValue<Vector2>();
            dashDirection = transform.right * moveInput.x + transform.forward * moveInput.y;
            
            if (dashDirection.magnitude < 0.1f)
            {
                dashDirection = transform.forward;
            }
            
            dashDirection.Normalize();
        }

        if (isDashing)
        {
            controller.Move(dashDirection * dashSpeed * Time.deltaTime);
            dashTimer -= Time.deltaTime;
            if (dashTimer <= 0)
            {
                isDashing = false;
            }
        }
    }

    private void ApplyGravity()
    {
        if (isGrounded && velocity.y < 0 && !isDashing)
        {
             velocity.y = -2f;
        }
        
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void HandleStamina()
    {
        if (staminaRegenTimer > 0)
        {
            staminaRegenTimer -= Time.deltaTime;
        }
        else if (currentStamina < maxStamina && !isDashing)
        {
            currentStamina += staminaRegenRate * Time.deltaTime;
            currentStamina = Mathf.Min(currentStamina, maxStamina);
            
            if (playerHealth != null)
            {
                playerHealth.UpdateStaminaUI(currentStamina, maxStamina);
            }
        }
    }

    private void UseStamina(float amount)
    {
        currentStamina -= amount;
        currentStamina = Mathf.Max(currentStamina, 0);
        staminaRegenTimer = staminaRegenDelay;

        if (playerHealth != null)
        {
            playerHealth.UpdateStaminaUI(currentStamina, maxStamina);
        }
    }
}