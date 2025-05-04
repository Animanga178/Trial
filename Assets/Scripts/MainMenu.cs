using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject menuCanvas;
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private GameObject helpPanel;

    private void Start()
    {
        AudioManager.Instance.PlayMusic(AudioManager.Instance.menuMusic);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LeaderboardToMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 2);
    }

    public void MenuToLeaderboard()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
    }

    public void GameToMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void OpenOptions()
    {
        optionsPanel.SetActive(true);
        menuCanvas.SetActive(false);
    }

    public void CloseOptions()
    {
        optionsPanel.SetActive(false);
        menuCanvas.SetActive(true);
    }

    public void OpenHelp()
    {
        helpPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void CloseHelp()
    {
        helpPanel.SetActive(false);
        Time.timeScale = 1f;
    }
}
