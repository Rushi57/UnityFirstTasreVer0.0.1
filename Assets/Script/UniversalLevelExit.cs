using UnityEngine;
using UnityEngine.SceneManagement;

public class UniversalLevelExit : MonoBehaviour
{
    [SerializeField] private string mapSceneName = "Map"; // optional fallback

    public void GoToMap()
    {
       

        string mapToLoad = string.IsNullOrEmpty(GameSession.MapSceneName)
            ? mapSceneName
            : GameSession.MapSceneName;

        Debug.Log($"[UniversalLevelExit] Loading map scene: {mapToLoad}");
        SceneManager.LoadScene(mapToLoad);
    }
}
