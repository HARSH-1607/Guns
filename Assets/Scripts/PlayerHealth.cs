using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("UI Elements")]
    public Image healthBarForeground;
    public TextMeshProUGUI healthText;
    public Image damageOverlay;  // Reference to the damage overlay image

    private void Start()
    {
        InitializeHealth();
    }

    private void Update()
    {
        HandleHealthInput();
        CheckLowHealth();
    }

    // Initialize health values
    private void InitializeHealth()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    // Handle health input for increasing and decreasing health
    private void HandleHealthInput()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            Heal(10);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            TakeDamage(10);
        }
    }

    // Method to take damage
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Method to heal the player
    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();
    }

    // Update the health UI
    private void UpdateHealthUI()
    {
        healthBarForeground.fillAmount = (float)currentHealth / maxHealth;
        healthText.text = $"{currentHealth} / {maxHealth}";
    }

    // Check and display low health overlay
    private void CheckLowHealth()
    {
        if (currentHealth <= maxHealth * 0.25f) // 25% threshold for low health
        {
            // Make the overlay visible
            damageOverlay.color = new Color(1, 0, 0, Mathf.PingPong(Time.time * 2, 1)); // Flashing red
        }
        else
        {
            // Hide the overlay
            damageOverlay.color = new Color(1, 0, 0, 0);
        }
    }

    // Handle player death
    private void Die()
    {
        Debug.Log("Player has died!");
        // Additional death logic can be added here
    }
}
