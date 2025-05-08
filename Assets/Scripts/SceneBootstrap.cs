using System.Collections;
using UnityEngine;

public class SceneBootstrap : MonoBehaviour
{
    [SerializeField] private GameObject loadingPanel;
    private GameManager gameManager;

    private void Start()
    {
        StartLoadingSequence();
    }

    public void StartLoadingSequence()
    {
        StartCoroutine(StartGameDelay());
    }

    private IEnumerator StartGameDelay()
    {
        loadingPanel.SetActive(true);
        yield return new WaitUntil(() => GameManager.Instance != null);

        gameManager = GameManager.Instance;
        gameManager.NewGame();

        yield return new WaitForSeconds(3f);

        loadingPanel.SetActive(false);
    }
}
