using UnityEngine;

public class PowerUpPickup : MonoBehaviour
{
    [SerializeField] private PowerUpType powerUpType;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerController player = other.GetComponent<PlayerController>();

        if (powerUpType == PowerUpType.BlackOut)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.blackOutPUsound, 2f);
            new BlackOutDebuff().Execute(player);
        }
        else
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.powerUpPickupSound);
            InventoryManager.Instance.AddPowerUp(powerUpType.ToString());
        }
        Destroy(gameObject);
    }
}

