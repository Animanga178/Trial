using UnityEngine;

public class LuckyDipPowerUp : PowerUpCommand
{
    public override void Execute(PlayerController player)
    {
        int roll = Random.Range(0, 2);

        if (roll == 0)
        {
            GameManager.Instance.AddTime(7);
        }
        else
        {
            GameManager.Instance.SetLives(GameManager.Instance.lives + 1);
        }
    }
}
