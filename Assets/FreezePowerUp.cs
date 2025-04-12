using System.Collections;
using UnityEngine;

public class FreezePowerUp : PowerUpCommand
{
    private float duration;

    public FreezePowerUp(float duration)
    {
        this.duration = duration;
    }

    public override void Execute(PlayerController player)
    {
        Debug.Log("ActivateFreeze called");
        player.StartCoroutine(FreezeCoroutine(duration));
    }

    private IEnumerator FreezeCoroutine(float duration)
    {
        Debug.Log("FreezeCoroutine called");

        ObstacleMovement.GlobalFreeze = true;

        ObstacleMovement[] obstacles = Object.FindObjectsByType<ObstacleMovement>(FindObjectsSortMode.None);
        foreach (var o in obstacles)
        {
            Debug.Log($"Found obstacle: {o.name}, Tag: {o.tag}");

            if (o != null && o.IsOnWater())
            {
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
