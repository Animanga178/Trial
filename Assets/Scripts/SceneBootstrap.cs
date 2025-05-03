using System.Collections;
using UnityEngine;

public class SceneBootstrap : MonoBehaviour
{
    [SerializeField] private GameObject loadingPanel;
    [SerializeField] private GameManager gameManager;

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
        gameManager.NewGame();

        yield return new WaitForSeconds(3f);

        loadingPanel.SetActive(false);
    }
}
