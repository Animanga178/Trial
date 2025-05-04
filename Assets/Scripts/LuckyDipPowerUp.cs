using UnityEngine;

public class LuckyDipPowerUp : PowerUpCommand
{
    private GameObject luckyDipEffect;

    public LuckyDipPowerUp(GameObject luckyDipEffect)
    {
        this.luckyDipEffect = luckyDipEffect;
    }

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
        AudioManager.Instance.PlayPowerUpSound(PowerUpType.LuckyDip);
        Effect(player);
    }

    public void Effect(PlayerController player)
    {
        GameObject effect = Object.Instantiate(luckyDipEffect, player.transform.position, Quaternion.identity);
        Object.Destroy(effect, 0.5f);
    }
}
