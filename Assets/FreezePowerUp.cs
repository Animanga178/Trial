using UnityEngine;

public class FreezePowerUp : PowerUpCommand
{
    public override void Execute(PlayerController player)
    {
        player.ActivateFreeze(5f);
    }
}
