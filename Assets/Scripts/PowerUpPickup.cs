using UnityEngine;

public class PowerUpPickup : MonoBehaviour
{
    [SerializeField] private string powerUpName;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.powerUpPickupSound);
            InventoryManager.Instance.AddPowerUp(powerUpName);
            Destroy(gameObject);
        }
    }
}

