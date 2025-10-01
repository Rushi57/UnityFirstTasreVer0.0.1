using UnityEngine;
using UnityEngine.UI;

public class LevelStarDisplay : MonoBehaviour
{
    [Header("Star Images")]
    public Image[] stars; // Assign 3 star icons in inspector
    public string levelName = "Lvl1"; // Which level this star display belongs to

    void Start()
    {
        string levelKey = levelName + "_Stars";
        int earnedStars = PlayerPrefs.GetInt(levelKey, 0);

        // Reset all to empty
        for (int i = 0; i < stars.Length; i++)
            stars[i].enabled = false;

        // Show earned stars
        for (int i = 0; i < earnedStars; i++)
            stars[i].enabled = true;
    }
}
