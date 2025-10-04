using UnityEngine;

public class LevelResultSaver
{
    //Save Score and Start for level 
    public static void SaveResult(int levelIndex, int score, int stars)
    {

        string scoreKey = $"Level{levelIndex}_Score";
        string starKey = $"Level{levelIndex}_Stars";

        //Save Score(Overwrite)
        PlayerPrefs.SetInt(scoreKey, score);

        // Keep best stars (only overwrite if new stars > old)
        int prevStars = PlayerPrefs.GetInt(starKey, 0);
        if(stars > prevStars)
            PlayerPrefs.SetInt(starKey, prevStars);

        PlayerPrefs.Save();
    }

    public static int LoadStars(int levelIndex)
    {
        return PlayerPrefs.GetInt($"Level{levelIndex}", 0);
    }
    public static int LoadScore(int levelIndex)
    {
        return PlayerPrefs.GetInt($"Level{levelIndex}", 0); 
    }
}
