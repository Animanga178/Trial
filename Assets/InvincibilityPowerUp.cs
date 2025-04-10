using UnityEngine;

public class InvincibilityPowerUp : PowerUpCommand
{
    public override void Execute(PlayerController player)
    {
        player.ActivateInvincibility(7);
    }
}
