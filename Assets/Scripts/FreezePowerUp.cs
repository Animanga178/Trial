using System.Collections;
using UnityEngine;

public class FreezePowerUp : PowerUpCommand
{
    private float duration;

    private GameObject freezeEffect;
    public FreezePowerUp(float duration, GameObject freezeEffect)
    {
        this.duration = duration;
        this.freezeEffect = freezeEffect;
    }

    public override void Execute(PlayerController player)
    {
        player.StartCoroutine(FreezeCoroutine(duration));
    }

    private IEnumerator FreezeCoroutine(float duration)
    {
        AudioManager.Instance.PlayPowerUpSound(PowerUpType.Freeze);
        ObstacleMovement.GlobalFreeze = true;

        ObstacleMovement[] obstacles = Object.FindObjectsByType<ObstacleMovement>(FindObjectsSortMode.None);
        foreach (var o in obstacles)
        {
            if (o != null && o.IsOnWater())
            {
                GameObject effect = Object.Instantiate(freezeEffect, o.transform.position, Quaternion.identity);
                Object.Destroy(effect, 1f);
                o.Freeze();
            }
        }
        yield return new WaitForSeconds(duration);

        ObstacleMovement[] frozenObstacles = Object.FindObjectsByType<ObstacleMovement>(FindObjectsSortMode.None);
        foreach (var o in frozenObstacles)
        {
            if (o != null && o.CompareTag("OnWater"))
            {
                o.Unfreeze();
            }
        }

        ObstacleMovement.GlobalFreeze = false;
    }
}
