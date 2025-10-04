using UnityEngine;

public static class LevelResultSaver
{
    // Save Score and Stars for level
    public static void SaveResult(int levelIndex, int score, int stars)
    {
        string scoreKey = $"Level{levelIndex}_Score";
        string starKey = $"Level{levelIndex}_Stars";

        // Save score (always overwrite)
        PlayerPrefs.SetInt(scoreKey, score);

        // Save stars (only if it's higher than the previous)
        int prevStars = PlayerPrefs.GetInt(starKey, 0);
        if (stars > prevStars)
            PlayerPrefs.SetInt(starKey, stars);

        PlayerPrefs.Save();

        Debug.Log($"[LevelResultSaver] Saved Level {levelIndex}: Score={score}, Stars={stars}");
    }

    public static int LoadStars(int levelIndex)
    {
        return PlayerPrefs.GetInt($"Level{levelIndex}_Stars", 0);
    }

    public static int LoadScore(int levelIndex)
    {
        return PlayerPrefs.GetInt($"Level{levelIndex}_Score", 0);
    }
}
