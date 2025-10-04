using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TotalScoreManager : MonoBehaviour
{
    public static TotalScoreManager Instance;

    private int mixScore;
    private int cutScore;
    private int simmerScore;

    [Header("Panels")]
    [SerializeField] private GameObject completeDishPanel;   // Congrats Panel
    [SerializeField] private GameObject scoreDashBoardPanel; // Score Panel

    [Header("UI References (Score Panel)")]
    [SerializeField] private TextMeshProUGUI mixingText;
    [SerializeField] private TextMeshProUGUI cuttingText;
    [SerializeField] private TextMeshProUGUI simmerText;
    [SerializeField] private TextMeshProUGUI totalScoreText;

    [Header("Dish UI Reference (Score Panel)")]
    [SerializeField] private Image dishImageDisplay;   // assign ScorePanel's dish image
    [SerializeField] private TextMeshProUGUI dishNameText;

    [Header("Star References")]
    [SerializeField] private Image[] stars;
    [SerializeField] private Color starFilledColor = Color.yellow;
    [SerializeField] private Color starEmptyColor = Color.gray;

    [Header("Target Scores (tweak per-level in inspector or code)")]
    [SerializeField] private int targetScore = 300; // 3★ threshold

    [Header("Map/Level Settings")]
    [SerializeField] private string mapSceneName = "Map"; // change to your map scene name
    [SerializeField] private int currentLevelIndex = 0; // set to 0 for Lvl1, 1 for Lvl2...

    public int FinalScore { get; private set; }
    private RecipeSO lastRecipe; // store recipe used (so scoreboard shows image/title)
    private int lastStarCount = 0;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // ------------- Score registration -------------
    public void AddMixScore(int amount) => mixScore += amount;
    public void AddCutScore(int amount) => cutScore += amount;
    public void AddSimmerScore(int amount) => simmerScore += amount;

    // Called once when recipe is done - pass the RecipeSO so we can show dish image/title later
    public void CalculateFinalScore(string recipeName, RecipeSO recipe)
    {
        // Optional: ensure recipe finished check (if you have CookingStepManager)
        // if (!CookingStepManager.Instance.IsRecipeCompleted()) return;

        FinalScore = mixScore + cutScore + simmerScore;
        lastRecipe = recipe;

        // Set the per-level targetScore from the recipe
        if (recipe != null)
            targetScore = recipe.targetScore;

        Debug.Log($"[TotalScoreManager] Final Score for {recipe.recipeName}: {FinalScore} / Target {targetScore}");

        if (completeDishPanel != null)
            completeDishPanel.SetActive(true);
    }

    // Show scoreboard (called when tap/click on Congrats panel)
    public void ShowScoreBoard()
    {
        if (completeDishPanel != null) completeDishPanel.SetActive(false);
        if (scoreDashBoardPanel != null) scoreDashBoardPanel.SetActive(true);

        UpdateScoreUI();
        lastStarCount = CalculateStarCount();
        UpdateStars(lastStarCount);
        UpdateDishInfo();
    }

    private void UpdateScoreUI()
    {
        if (mixingText != null) mixingText.text = mixScore.ToString();
        if (cuttingText != null) cuttingText.text = cutScore.ToString();
        if (simmerText != null) simmerText.text = simmerScore.ToString();
        if (totalScoreText != null) totalScoreText.text = FinalScore.ToString();
    }

    private int CalculateStarCount()
    {
        int middleScore = targetScore / 2;
        if (FinalScore >= targetScore) return 3;
        if (FinalScore >= middleScore) return 2;
        if (FinalScore > 0) return 1;
        return 0;
    }

    private void UpdateStars(int starCount)
    {
        for (int i = 0; i < stars.Length; i++)
            stars[i].color = (i < starCount) ? starFilledColor : starEmptyColor;
    }

    private void UpdateDishInfo()
    {
        if (lastRecipe != null)
        {
            if (dishNameText != null) dishNameText.text = lastRecipe.recipeName;
            if (dishImageDisplay != null && lastRecipe.recipeImage != null)
                dishImageDisplay.sprite = lastRecipe.recipeImage;
        }
    }

    // Called by Continue button on the Score Dashboard
    public void OnContinueAndReturnToMap()
    {
        int stars = CalculateStarCount();
        LevelResultSaver.SaveResult(currentLevelIndex, FinalScore, stars);

        Debug.Log($"Saved Level {currentLevelIndex} → Score: {FinalScore}, Stars: {stars}");

        ResetScores();
        SceneManager.LoadScene(mapSceneName);
    }

    // Reset gameplay scores (call this in Start of level to ensure fresh run)
    public void ResetScores()
    {
        mixScore = 0;
        cutScore = 0;
        simmerScore = 0;
        FinalScore = 0;
        lastRecipe = null;
        lastStarCount = 0;

        // Hide panels
        if (completeDishPanel != null) completeDishPanel.SetActive(false);
        if (scoreDashBoardPanel != null) scoreDashBoardPanel.SetActive(false);
    }
}
