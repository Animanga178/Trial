using TMPro;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardUI : MonoBehaviour
{
    [SerializeField] private Transform entryContainer;
    [SerializeField] private GameObject entryPrefab;
    [SerializeField] private Sprite goldMedal;
    [SerializeField] private Sprite silverMedal;
    [SerializeField] private Sprite bronzeMedal;

    private void Start()
    {
        DisplayLeaderboard();

        if (AudioManager.Instance != null)
        {
            Debug.Log("Current music clip: " + AudioManager.Instance?.GetCurrentClipName());
            AudioManager.Instance.StopMusic();
            AudioManager.Instance.PlayMusic(AudioManager.Instance.leaderboardMusic);
        }
    }

    public void DisplayLeaderboard()
    {
        List<LeaderboardEntry> entries = LeaderboardManager.Load();

        foreach (Transform child in entryContainer)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < entries.Count; i++)
        {
            GameObject item = Instantiate(entryPrefab, entryContainer);
            LeaderboardEntry entry = entries[i];
            Image medalImage = item.transform.Find("Frame/MedalIcon").GetComponent<Image>();

            if (i == 0)
                medalImage.sprite = goldMedal;
            else if (i < 5)
                medalImage.sprite = silverMedal;
            else if (i < 10)
                medalImage.sprite = bronzeMedal;
            else
                medalImage.enabled = false;

            item.transform.Find("Frame/Rank").GetComponent<TextMeshProUGUI>().text = (i + 1).ToString();
            item.transform.Find("Frame/Name").GetComponent<TextMeshProUGUI>().text = entry.playerName;
            item.transform.Find("Frame/Score").GetComponent<TextMeshProUGUI>().text = entry.score.ToString("D5");

            string currentPlayerName = LeaderboardManager.CurrentPlayerName;

            if (entry.playerName == currentPlayerName)
            {
                item.transform.Find("Frame").GetComponent<Image>().color = new Color(1f, 0.92f, 0.016f, 0.6f);
            }

        }
    }
}
