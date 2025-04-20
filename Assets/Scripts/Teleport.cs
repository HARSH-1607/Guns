using UnityEngine;
using TMPro;

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
            if (Input.GetKeyDown(KeyCode.C))
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
        // Teleport the player to the specified location
        player.position = teleportLocation;
        Debug.Log("Player teleported to: " + teleportLocation); // Log to confirm teleportation
    }
}
