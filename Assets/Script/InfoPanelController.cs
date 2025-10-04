using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class InfoPanelController : MonoBehaviour
{
    [Header("UI References")]
    public Image dishImageDisplay;
    public TextMeshProUGUI dishTitleDisplay;
    public TextMeshProUGUI scoreText;
    public Image[] starIcons; // 3 stars
    public Color filledColor = Color.yellow;
    public Color emptyColor = Color.gray;

    [Header("Buttons")]
    public Button playButton;
    public Button backButton;

    private RecipeSO currentRecipe;
    private int currentLevelIndex;

    public void Setup(RecipeSO recipe, int index)
    {
        currentRecipe = recipe;
        currentLevelIndex = index;

        // Display recipe info
        if (dishImageDisplay != null) dishImageDisplay.sprite = recipe.recipeImage;
        if (dishTitleDisplay != null) dishTitleDisplay.text = recipe.recipeName;

        // Load saved score + stars
        int levelScore = PlayerPrefs.GetInt($"Level{index}_Score", 0);
        int stars = PlayerPrefs.GetInt($"Level{index}_Stars", 0);

        if (levelScore > 0)
        {
            scoreText.gameObject.SetActive(true);
            scoreText.text = $"Score: {levelScore}";
        }
        else
        {
            scoreText.gameObject.SetActive(false);
        }

        // Update stars
        for (int i = 0; i < starIcons.Length; i++)
        {
            if (starIcons[i] != null)
                starIcons[i].color = (i < stars) ? filledColor : emptyColor;
        }

        // Hook Play button
        if (playButton != null)
        {
            playButton.onClick.RemoveAllListeners();
            playButton.onClick.AddListener(() => LoadLevel());
        }

        // Hook Back button
        if (backButton != null)
        {
            backButton.onClick.RemoveAllListeners();
            backButton.onClick.AddListener(() => ClosePanel());
        }
    }

    private void LoadLevel()
    {
        PlayerPrefs.SetInt("CurrentLevelIndex", currentLevelIndex);
        SceneManager.LoadScene($"Level{currentLevelIndex + 1}");
        // assumes your scenes are named Level1, Level2,
    }

    private void ClosePanel()
    {
        Animator anim = GetComponent<Animator>();
        if (anim != null)
        {
            anim.SetTrigger("Closed");
            Destroy(gameObject, 0.5f); // delay so animation can play
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
