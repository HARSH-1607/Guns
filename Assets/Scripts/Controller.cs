using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 5.0f;
    public float runSpeed = 8.0f;
    public float crouchSpeed = 2.5f;
    public float jumpHeight = 1.5f;
    public float gravity = -9.81f;
    
    [Header("Crouch Settings")]
    public float crouchHeight = 1.0f;
    public float crouchTransitionSpeed = 8.0f;  // Speed of height transition
    private float originalHeight;
    private Vector3 originalCenter;
    private bool isCrouching = false;
    private bool isTransitioningCrouch = false;

    [Header("Mouse Settings")]
    public float mouseSensitivity = 100f;
    private float xRotation = 0f;

    private CharacterController controller;
    private Transform playerCamera;
    private Vector3 velocity;
    private bool isGrounded;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        playerCamera = Camera.main.transform;

        originalHeight = controller.height;
        originalCenter = controller.center;

        // Lock cursor to the center of the screen
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        HandleMouseLook();
        HandleMovement();
        HandleJumping();
        HandleCrouching();
        ApplyGravity();
    }

    // Handle mouse look functionality
    private void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    // Handle player movement input
    private void HandleMovement()
    {
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Ensure the player stays grounded
        }

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        Vector3 move = transform.right * moveX + transform.forward * moveZ;

        // Determine speed based on sprinting or crouching
        float speed = walkSpeed;
        if (Input.GetKey(KeyCode.LeftShift) && !isCrouching)
        {
            speed = runSpeed;
        }
        else if (isCrouching)
        {
            speed = crouchSpeed;
        }

        controller.Move(move * speed * Time.deltaTime);
    }

    // Handle jumping input
    private void HandleJumping()
    {
        if (Input.GetButtonDown("Jump") && isGrounded && !isCrouching)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    // Handle crouching input with smooth transition
    private void HandleCrouching()
    {
        if (Input.GetKeyDown(KeyCode.X))  // Changed crouch button to 'X'
        {
            isCrouching = !isCrouching;
            isTransitioningCrouch = true;
        }

        if (isTransitioningCrouch)
        {
            float targetHeight = isCrouching ? crouchHeight : originalHeight;
            float targetCenterY = isCrouching ? crouchHeight / 2 : originalCenter.y;

            // Smoothly interpolate height and center
            controller.height = Mathf.Lerp(controller.height, targetHeight, Time.deltaTime * crouchTransitionSpeed);
            controller.center = Vector3.Lerp(controller.center, new Vector3(controller.center.x, targetCenterY, controller.center.z), Time.deltaTime * crouchTransitionSpeed);

            // Check if the transition is complete
            if (Mathf.Abs(controller.height - targetHeight) < 0.05f)
            {
                controller.height = targetHeight;
                controller.center = new Vector3(controller.center.x, targetCenterY, controller.center.z);
                isTransitioningCrouch = false;
            }
        }
    }

    // Apply gravity to the player
    private void ApplyGravity()
    {
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
