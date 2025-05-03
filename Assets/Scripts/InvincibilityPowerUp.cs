using System.Collections;
using UnityEngine;

public class InvincibilityPowerUp : PowerUpCommand
{
    private GameObject invincibilityEffect;

    public InvincibilityPowerUp(GameObject invincibilityEffect)
    {
        this.invincibilityEffect = invincibilityEffect;
    }

    public override void Execute(PlayerController player)
    {
        player.StartCoroutine(InvincibilityCoroutine(player));
    }

    private IEnumerator InvincibilityCoroutine(PlayerController player)
    {
        SpriteRenderer spriteRenderer = player.GetComponent<SpriteRenderer>();
        float duration = 7f;
        float elapsed = 0f;

        player.SetInvincibility(true);

        GameObject effect = Object.Instantiate(invincibilityEffect, player.transform.position, Quaternion.identity);
        Object.Destroy(effect, 0.5f);

        while (elapsed < duration)
        {
            spriteRenderer.color = Color.magenta;
            yield return new WaitForSeconds(0.2f);
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(0.2f);
            elapsed += 0.4f;
        }

        player.SetInvincibility(false);
        spriteRenderer.color = Color.white;
    }
}
