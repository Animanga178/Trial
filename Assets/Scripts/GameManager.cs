using UnityEngine;
using System.Collections;
using TMPro;

public class GameManager : MonoBehaviour
{
    private Home[] homes;

    private PlayerController player;

    public GameObject gameOverMenu;

    private string currentPlayerName;

    private bool gameStarted = false;
    private bool scoreFlashed = false;

    [SerializeField] private TextMeshProUGUI livesText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI nextScoreText;
    [SerializeField] private Animator rankUpAnimator;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject pauseButton;

    public int lives { get; private set; } = 3;
    public int score { get; private set; } = 0;
    public int time { get; private set; } = 30;

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        homes = Object.FindObjectsByType<Home>(FindObjectsSortMode.None);
        player = FindFirstObjectByType<PlayerController>();
    }

    private void Start()
    {
        AudioManager.Instance.StopMusic();
        currentPlayerName = PlayerPrefs.GetString("PlayerName", "???");
        UpdateNextScoreUI();
    }

    public void Pause()
    {
        pausePanel.SetActive(true);
        pauseButton.SetActive(false);
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        pausePanel.SetActive(false);
        pauseButton.SetActive(true);
        Time.timeScale = 1f;
    }

    private void UpdateNextScoreUI()
    {
        int nextTarget = LeaderboardManager.GetNextTargetScore(score);
        Debug.Log($"Next score to beat: {nextTarget}");

        if (nextTarget > 0 && score < nextTarget)
        {
            nextScoreText.text = $"Next Rank: {nextTarget}";
            scoreText.color = Color.white;
            nextScoreText.color = Color.white;
            scoreFlashed = false;
            AudioManager.Instance.PlaySFX(AudioManager.Instance.leaderboardEntry);
        }
        else if (score >= nextTarget && nextTarget > 0)
        {
            nextScoreText.text = $"Next Rank: {nextTarget}";
            StartCoroutine(FlashText(nextScoreText, Color.cyan));
            StartCoroutine(FlashText(scoreText, Color.cyan));
            TriggerRankUpEffect();
            scoreFlashed = true;
            AudioManager.Instance.PlaySFX(AudioManager.Instance.leaderboardEntry);
        }
        else
        {
            nextScoreText.text = "You're #1!";
            if (!scoreFlashed)
            {
                AudioManager.Instance.PlaySFX(AudioManager.Instance.leaderboardEntry);
                StartCoroutine(FlashText(nextScoreText, Color.yellow));
                scoreFlashed = true;
            }
        }
    }

    private void TriggerRankUpEffect()
    {
        rankUpAnimator.SetTrigger("Play");
    }

    public void UnfreezeObstacles()
    {
        ObstacleMovement[] obstacles = Object.FindObjectsByType<ObstacleMovement>(FindObjectsSortMode.None);

        foreach (var obstacle in obstacles)
        {
            if (obstacle.CompareTag("OnWater"))
            {
                obstacle.Unfreeze();
            }
        }

        ObstacleMovement.GlobalFreeze = false;
    }

    public void NewGame()
    {
        gameStarted = true;

        gameOverMenu.SetActive(false);
        SetScore(0);
        SetLives(3);
        NewLevel();
    }

    private void NewLevel()
    {
        for (int i = 0; i < homes.Length; i++)
        {
            homes[i].enabled = false;
        }

        Respawn();
    }

    private void Respawn()
    {
        player.Respawn();
        StopAllCoroutines();
        StartCoroutine(SetTimer(30));
    }

    private IEnumerator SetTimer(int duration)
    {
        time = duration;
        timeText.text = time.ToString();

        yield return new WaitForSeconds(3f);

        while (time > 0)
        {
            yield return new WaitForSeconds(1);

            time--;
            timeText.text = time.ToString();
        }

        player.Death("Out of time");
    }

    public void MovedUp()
    {
        SetScore(score + 10);
    }

    public void PlayerDeath()
    {
        SetLives(lives - 1);

        if (lives > 0)
        {
            Invoke(nameof(Respawn), 1f);
        }
        else
        {
            Invoke(nameof(GameOver), 1f);
        }
    }

    private void GameOver()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.gameOverSFX);
        player.gameObject.SetActive(false);
        gameOverMenu.SetActive(true);

        StopAllCoroutines();
        StartCoroutine(PlayAgain());
    }

    private IEnumerator PlayAgain()
    {
        bool playAgain = false;

        while (!playAgain)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                playAgain = true;
            }
            yield return null;
        }

        StopAllCoroutines();

        FindFirstObjectByType<SceneBootstrap>().StartLoadingSequence();
    }

    private void SetScore(int score)
    {
        this.score = score;
        scoreText.text = score.ToString();
        Debug.Log($"Score updated to: {score}");
        UpdateNextScoreUI();
    }

    public void AddTime(int amount)
    {
        time += amount;
        timeText.text = time.ToString();
        StartCoroutine(FlashText(timeText, Color.green));
    }

    public void SetLives(int newLives)
    {
        if (!gameStarted) return;

        int previousLives = lives;
        lives = newLives;
        livesText.text = lives.ToString();

        if (newLives > previousLives && previousLives != 0)
        {
            StartCoroutine(FlashText(livesText, Color.green));
        }
        else if (newLives < previousLives)
        {
            StartCoroutine(FlashText(livesText, Color.red));
        }
    }

    public IEnumerator FlashText(TextMeshProUGUI text, Color flashColor)
    {
        Color original = text.color;
        text.color = flashColor;

        yield return new WaitForSecondsRealtime(0.6f);

        text.color = original;
    }

    public void HomeOccupied()
    {
        player.gameObject.SetActive(false);

        int bonus = time * 20;
        SetScore(score + bonus + 50);

        if (Cleared())
        {
            SetScore(score + 1000);
            Invoke(nameof(NewLevel), 1f);
        }
        else
        {
            Invoke(nameof(Respawn), 1f);
        }
    }

    private bool Cleared()
    {
        for (int i = 0; i < homes.Length; i++)
        {
            if (!homes[i].enabled)
            {
                return false;
            }
        }
        return true;
    }
}