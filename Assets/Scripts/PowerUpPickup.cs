using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PowerUpPickup : MonoBehaviour
{
    [SerializeField] private PowerUpType powerUpType;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        AudioManager.Instance.PlaySFX(AudioManager.Instance.powerUpPickupSound);
        PlayerController player = other.GetComponent<PlayerController>();

        if (powerUpType == PowerUpType.BlackOut)
        {
            new BlackOutDebuff().Execute(player);
        }
        else
        {
            InventoryManager.Instance.AddPowerUp(powerUpType.ToString());
        }
        Destroy(gameObject);
    }
}

