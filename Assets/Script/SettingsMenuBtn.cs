using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsMenuBtn : MonoBehaviour
{
    public GameObject SettingCanvas;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SettingCanvas.SetActive(false);
        
    }
    public void OpenSettigns()
    {
        SettingCanvas.SetActive(true);
        Time.timeScale = 1.0f;
    }

    public void CloseSettigs()
    {
        SettingCanvas.SetActive(false);
        Time.timeScale = 1.0f;
    }
    public void MainMenuScene()
    {
        SceneManager.LoadScene("MainMenu");
    }
   
}
