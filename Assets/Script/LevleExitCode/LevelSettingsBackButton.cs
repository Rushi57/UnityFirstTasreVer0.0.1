using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSettingsBackButton : MonoBehaviour
{
    [Header("Scene Setting ")]
    public string mapSceneName = "Map";

     private void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnBackButtonPressed);
    }

    private void OnBackButtonPressed()
    {
        SceneManager.LoadScene(mapSceneName);
    }
}
