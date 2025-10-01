using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class IndicatorController : MonoBehaviour
{
    public DishData dishData;
    public int levelIndex; // 0 = first level

    [Header("Info Panel Reference")]
    public GameObject infoPanel;
    public Image dishImageDisplay;
    public TextMeshProUGUI dishTitleDisplay;

    [Header("Button and Visual")]
    public Image buttonImage;
    public Color lockedColor = Color.gray;
    public Color unlockedColor = Color.white;

    [Header("Play Button")]
    public Button playButton;  // Assign in Inspector

    private Button button;

    // 👇 Your level scene names (must match scenes in Build Settings)
    private string[] levelScenes = new string[]
    {
        "Level1", "Level2", "Level3", "Level4", "Level5",
        "Level6", "Level7", "Level8", "Level9", "Level10"
    };

    private void Start()
    {
        button = GetComponent<Button>();
        SetupIndicator();

        if (playButton != null)
        {
            playButton.onClick.AddListener(LoadLevelScene);
        }
    }

    void SetupIndicator()
    {
        int previousScore = PlayerPrefs.GetInt($"Level{levelIndex - 1}_Score", 0);

        bool isUnlocked = (levelIndex == 0) || (previousScore >= 75);

        if (buttonImage != null)
            buttonImage.color = isUnlocked ? unlockedColor : lockedColor;
        if (button != null)
            button.interactable = isUnlocked;
    }

    public void ShowDishInfo()
    {
        if (dishData == null || infoPanel == null) return;

        infoPanel.SetActive(true);
        dishImageDisplay.sprite = dishData.dishImage;
        dishTitleDisplay.text = dishData.dishTitle;
    }

    void LoadLevelScene()
    {
        if (levelIndex >= 0 && levelIndex < levelScenes.Length)
        {
            string sceneToLoad = levelScenes[levelIndex];
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.LogWarning("⚠ Invalid Level Index: " + levelIndex);
        }
    }
}
