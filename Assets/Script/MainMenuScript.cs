using UnityEditor.Overlays;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public GameObject SettingCanvas;
    public GameObject CreditsCanvas;
    public GameObject LoadGameCanvas;

    [SerializeField] private string firstGameScene = "Map";
    [SerializeField] private int totalSlots = 3; //Number of slots
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SettingCanvas.SetActive(false);
        CreditsCanvas.SetActive(false);

    }
    //Settings
    public void OpenSettigns()
    {
        SettingCanvas.SetActive(true);
      
    }

    public void CloseSettigs()
    {
        SettingCanvas.SetActive(false);
        
    }
    //Credits
    public void OpenCredists()
    {
        CreditsCanvas.SetActive(true) ;
    }
    public void CloseCredists()
    {
        CreditsCanvas.SetActive(false);
    }
    //LoadGame
    public void OpenLoadGame()
    {
        LoadGameCanvas.SetActive(true);
    }
    public void CloseLoadGame()
    {
        LoadGameCanvas.SetActive(false);
    }
    //newGame
    public void NewGame()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt("TutorialDone", 0);
        PlayerPrefs.Save();

        SceneManager.LoadScene(firstGameScene);
    }

    //Close Game
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("QuitGAME");
    }

}
