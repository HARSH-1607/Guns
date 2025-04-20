using UnityEngine;
using TMPro;

public class KeypadController : MonoBehaviour
{
    [Header("Door Settings")]
    public Animator doorAnimator;                  // Reference to the door's Animator
    public GameObject interactionTextUI;           // UI Text displayed for interaction prompt
    public TMP_Text interactionText;               // Text component to show the interaction message
    public string openPrompt = "Press E to open door"; // Message shown when player is near
    public float interactionDistance = 3f;         // Distance within which the player can interact

    private Transform player;                      // Reference to the player's transform
    private bool doorOpen = false;                 // Track if the door is currently open

    void Start()
    {
        // Find the player in the scene
        player = GameObject.FindGameObjectWithTag("Player").transform;
        interactionTextUI.SetActive(false);        // Hide interaction text at start
    }

    void Update()
    {
        // Check the distance between the player and the keypad
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);

        if (distanceToPlayer <= interactionDistance)
        {
            // Show interaction text when player is within interaction distance
            interactionTextUI.SetActive(true);
            interactionText.text = openPrompt;

            // Open or close the door when player presses 'E' and is near the keypad
            if (Input.GetKeyDown(KeyCode.E))
            {
                ToggleDoor();
            }
        }
        else
        {
            // Hide interaction text when player is out of range
            interactionTextUI.SetActive(false);
        }
    }

    private void ToggleDoor()
    {
        doorOpen = !doorOpen;
        doorAnimator.SetBool("IsOpen", doorOpen);   // Trigger door open/close animation
    }
}
