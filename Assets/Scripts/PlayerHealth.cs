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

    [Header("Stamina UI")]
    public Image staminaBarForeground; // Reference for the yellow bar

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
        
        // MODIFIED: Added 'R' key to heal to full
        if (Input.GetKeyDown(KeyCode.R))
        {
            Heal(maxHealth); // This will heal the player to max health
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
        if(healthBarForeground != null)
        {
            healthBarForeground.fillAmount = (float)currentHealth / maxHealth;
        }
        if(healthText != null)
        {
            healthText.text = $"{currentHealth} / {maxHealth}";
        }
    }

    // Public function to update the stamina bar
    public void UpdateStaminaUI(float currentStamina, float maxStamina)
    {
        if (staminaBarForeground != null)
        {
            staminaBarForeground.fillAmount = currentStamina / maxStamina;
        }
    }

    // Check and display low health overlay
    private void CheckLowHealth()
    {
        if (damageOverlay == null) return;

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