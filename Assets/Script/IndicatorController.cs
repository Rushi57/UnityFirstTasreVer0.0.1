// IndicatorController.cs
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class IndicatorController : MonoBehaviour
{
    [Header("Recipe")]
    public RecipeSO recipe; // assign the RecipeSO for this indicator

    [Header("UI")]
    public Image dishImage;
    public TextMeshProUGUI dishTitle;
    public Image[] starIcons; // 3 star images
    public Color starFilledColor = Color.yellow;
    public Color starEmptyColor = Color.gray;

    [Header("Buttons")]
    public Button playButton; // assign
    public Button infoButton; // assign

    [Header("Info Panel")]
    public GameObject infoPanelPrefab; // assign your InfoPanel prefab

    private void Start()
    {
        UpdateVisuals();
        if (playButton != null) playButton.onClick.AddListener(OnPlayClicked);
        if (infoButton != null) infoButton.onClick.AddListener(OnInfoClicked);
    }

    private void UpdateVisuals()
    {
        if (recipe == null) return;

        if (dishImage != null && recipe.recipeImage != null) dishImage.sprite = recipe.recipeImage;
        if (dishTitle != null) dishTitle.text = recipe.recipeName;

        UpdateStarsFromSave();
    }

    private string GetRecipeKey()
    {
        return "Stars_" + (string.IsNullOrEmpty(recipe.recipeID) ? recipe.recipeName : recipe.recipeID);
    }

    public void UpdateStarsFromSave()
    {
        if (recipe == null || starIcons == null) return;

        int saved = PlayerPrefs.GetInt(GetRecipeKey(), 0);
        for (int i = 0; i < starIcons.Length; i++)
        {
            if (starIcons[i] == null) continue;
            starIcons[i].color = (i < saved) ? starFilledColor : starEmptyColor;
        }
    }

    private void OnPlayClicked()
    {
        if (recipe == null) return;

        // store selected recipe for the gameplay scene
        GameSession.SelectedRecipe = recipe;

        // reset runtime scores in TotalScoreManager (if present and persistent)
        if (TotalScoreManager.Instance != null) TotalScoreManager.Instance.ResetScores();

        // load the gameplay scene (set GameSession.GameplaySceneName or change below)
        SceneManager.LoadScene(GameSession.GameplaySceneName);
    }

    private void OnInfoClicked()
    {
        if (infoPanelPrefab == null || recipe == null) return;

        var canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            Debug.LogWarning("No Canvas found in scene to parent InfoPanel to.");
            return;
        }

        var panel = Instantiate(infoPanelPrefab, canvas.transform);
        var ctrl = panel.GetComponent<InfoPanelController>();
        if (ctrl != null)
        {
            ctrl.Setup(recipe, OnPlayClicked); // Play button in info panel calls same OnPlay
        }
    }
}
