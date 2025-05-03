using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private TMP_InputField nameInput;
    [SerializeField] private Button submitButton;

    private void Start()
    {
        submitButton.interactable = false;
        nameInput.characterLimit = 10;

        nameInput.onValueChanged.AddListener(OnNameChanged);
        submitButton.onClick.AddListener(SubmitScore);
    }

    private void OnNameChanged(string input)
    {
        submitButton.interactable = input.Length >= 3;
    }

    private void SubmitScore()
    {
        string name = nameInput.text.ToUpper();
        int finalScore = GameManager.Instance.score;

        LeaderboardManager.AddScore(name, finalScore);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
