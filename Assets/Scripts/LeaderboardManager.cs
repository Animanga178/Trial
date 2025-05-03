using UnityEngine;
using System.Collections.Generic;
using System.IO;

public static class LeaderboardManager
{
    public static string CurrentPlayerName { get; private set; }

    private static string savePath = Path.Combine(Application.persistentDataPath, "leaderboard.json");

    // Wrapped in a class so we can convert it to and from JSON
    [System.Serializable]
    private class Wrapper
    {
        public List<LeaderboardEntry> entries = new List<LeaderboardEntry>();
    }

    public static List<LeaderboardEntry> Load()
    {
        if (!File.Exists(savePath))
        {
            Debug.Log("Leaderboard file does not exist. Returning empty list.");
            return new List<LeaderboardEntry>();
        }

        string json = File.ReadAllText(savePath);
        Debug.Log("Leaderboard JSON: " + json);

        // Converts list of scores to JSON 
        Wrapper wrapper = JsonUtility.FromJson<Wrapper>(json);
        if (wrapper == null)
        {
            Debug.LogError("Failed to deserialize leaderboard JSON.");
            return new List<LeaderboardEntry>();
        }
        if (wrapper.entries == null)
        {
            Debug.LogError("Leaderboard entries are null.");
            return new List<LeaderboardEntry>();
        }

        Debug.Log("Loaded leaderboard entries: " + wrapper.entries.Count);
        foreach (LeaderboardEntry entry in wrapper.entries)
        {
            Debug.Log(entry.playerName + ": " + entry.score);
        }
        return wrapper.entries;
    }

    public static void Save(List<LeaderboardEntry> entries)
    {
        Debug.Log("Saving leaderboard to: " + savePath);

        foreach (LeaderboardEntry entry in entries)
        {
            Debug.Log(entry.playerName + ": " + entry.score);
        }
        // Wraps list of entries in Wrapper object
        // Converts list of scores to JSON
        string json = JsonUtility.ToJson(new Wrapper { entries = entries }, true);
        Debug.Log("Saving leaderboard JSON: " + json);
        // Write scores to a save file
        File.WriteAllText(savePath, json);
    }

    public static void AddScore(string playerName, int score)
    {
        CurrentPlayerName = playerName;

        var entries = Load();
        entries.Add(new LeaderboardEntry { playerName = playerName, score = score });
        entries.Sort((a, b) => b.score.CompareTo(a.score));
        Save(entries);
    }

    public static int GetNextTargetScore(int currentScore)
    {
        var entries = Load();
        entries.Add(new LeaderboardEntry { playerName = "YOU", score = currentScore });
        entries.Sort((a, b) => b.score.CompareTo(a.score));

        for (int i = 0; i < entries.Count; i++)
        {
            if (entries[i].playerName == "YOU" && i > 0)
            {
                return entries[i - 1].score;
            }
        }

        return -1;
    }
}