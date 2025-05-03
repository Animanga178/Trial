using TMPro;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardUI : MonoBehaviour
{
    [SerializeField] private Transform entryContainer;
    [SerializeField] private GameObject entryPrefab;

    private void Start()
    {
        DisplayLeaderboard();
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
