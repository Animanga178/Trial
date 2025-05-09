using UnityEngine;
using System.Collections;
using UnityEngine.Rendering.Universal;

public class BlackOutDebuff : PowerUpCommand
{
    private Light2D globalLight;
    private Light2D playerLight;
    private Coroutine blackOutCoroutine;

    public override void Execute(PlayerController player)
    {
        globalLight = GameObject.Find("Global Light 2D").GetComponent<Light2D>();
        playerLight = player.GetComponentInChildren<Light2D>(true);

        if (player == null) return;

        player.BlackOutDebuffInstance = this;

        if (blackOutCoroutine != null)
        {
            player.StopCoroutine(blackOutCoroutine);
        }


        blackOutCoroutine = player.StartCoroutine(BlackOutCoroutine());
    }

    private IEnumerator BlackOutCoroutine()
    {
        if (globalLight != null) globalLight.intensity = 0f;
        if (playerLight != null) playerLight.enabled = true;

        yield return new WaitForSeconds(5f);

        LightsOn();
    }

    public void LightsOn()
    {
        if (playerLight != null) playerLight.enabled = false;
        if (globalLight != null) globalLight.intensity = 1f;
    }
}
