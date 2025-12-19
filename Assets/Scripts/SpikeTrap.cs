using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    [Header("Trap Settings")]
    public int damageAmount = 20;       // How much damage to deal
    public float damageCooldown = 1.0f; // Time between damage ticks (in seconds)

    private float lastDamageTime;       // Timer to track the cooldown

    // This function is called continuously while another collider stays inside this object's trigger
    private void OnTriggerStay(Collider other)
    {
        // Check if the object is the "Player" and if the cooldown has passed
        if (other.CompareTag("Player") && Time.time > lastDamageTime + damageCooldown)
        {
            // Find the PlayerHealth script on the player
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

            // If the player has a health script...
            if (playerHealth != null)
            {
                // ...deal damage and reset the timer
                playerHealth.TakeDamage(damageAmount);
                lastDamageTime = Time.time;
            }
        }
    }

    // Optional: Reset the timer if the player leaves and comes back
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            lastDamageTime = 0;
        }
    }
}