using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelCompleteHandler : MonoBehaviour
{
    [Header("References")]
    public string mapSceneName = "Map";  // Change if your map scene name is different
    private int starsEarned = 0;

    // Call this when Continue button is pressed
    public void OnContinueButton(int stars)
    {
        starsEarned = stars;

        // Save stars for this level
        string levelKey = SceneManager.GetActiveScene().name + "_Stars";
        PlayerPrefs.SetInt(levelKey, starsEarned);
        PlayerPrefs.Save();

        // Load back to Map scene
        SceneManager.LoadScene(mapSceneName);
    }
}
