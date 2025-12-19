using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class TeleportCube : MonoBehaviour
{
    [Header("Teleport Settings")]
    public Vector3 teleportLocation;             // The target location for teleportation
    public GameObject interactionTextUI;         // UI Text displayed for interaction prompt
    public TMP_Text interactionText;             // Text component to show the interaction message
    public string teleportPrompt = "Press C to teleport"; // Message shown when player is near
    public float interactionDistance = 3f;       // Distance within which the player can interact

    private Transform player;                    // Reference to the player's transform

    void Start()
    {
        // Find the player in the scene
        player = GameObject.FindGameObjectWithTag("Player").transform;
        interactionTextUI.SetActive(false);      // Hide interaction text at start
    }

    void Update()
    {
        // Check the distance between the player and the teleport cube
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);

        if (distanceToPlayer <= interactionDistance)
        {
            // Show interaction text when player is within interaction distance
            interactionTextUI.SetActive(true);
            interactionText.text = teleportPrompt;

            // Teleport the player when they press 'C' and are near the cube
            // Support both Legacy Input System and New Input System
            bool cPressed = false;
            
            // Check Legacy Input
            if (Input.GetKeyDown(KeyCode.C))
            {
                cPressed = true;
            }
            // Check New Input System
            else if (Keyboard.current != null && Keyboard.current.cKey.wasPressedThisFrame)
            {
                cPressed = true;
            }

            if (cPressed)
            {
                TeleportPlayer();
            }
        }
        else
        {
            // Hide interaction text when player is out of range
            interactionTextUI.SetActive(false);
        }
    }

    private void TeleportPlayer()
    {
        // Disable CharacterController if present to allow direct transform modification
        CharacterController controller = player.GetComponent<CharacterController>();
        bool wasEnabled = false;
        
        if (controller != null)
        {
            wasEnabled = controller.enabled;
            controller.enabled = false;
        }

        // Teleport the player to the specified location
        player.position = teleportLocation;

        // Re-enable CharacterController
        if (controller != null && wasEnabled)
        {
            controller.enabled = true;
        }

        Debug.Log("Player teleported to: " + teleportLocation); // Log to confirm teleportation
    }
}
