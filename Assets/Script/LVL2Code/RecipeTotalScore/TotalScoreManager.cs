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
    [SerializeField] private GameObject completeDishPanel;
    [SerializeField] private GameObject scoreDashBoardPanel;

    [Header("UI References (Score Panel)")]
    [SerializeField] private TextMeshProUGUI mixingText;
    [SerializeField] private TextMeshProUGUI cuttingText;
    [SerializeField] private TextMeshProUGUI simmerText;
    [SerializeField] private TextMeshProUGUI totalScoreText;

    [Header("Dish UI Reference (Score Panel)")]
    [SerializeField] private Image dishImageDisplay;
    [SerializeField] private TextMeshProUGUI dishNameText;

    [Header("Star References")]
    [SerializeField] private Image[] stars;
    [SerializeField] private Color starFilledColor = Color.yellow;
    [SerializeField] private Color starEmptyColor = Color.gray;

    [Header("Target Scores (tweak per-level in inspector or code)")]
    [SerializeField] private int targetScore = 300;

    [Header("Map/Level Settings")]
    [SerializeField] private string mapSceneName = "Map";
    [SerializeField] private int currentLevelIndex = 0;

    public int FinalScore { get; private set; }
    private RecipeSO lastRecipe;
    private int lastStarCount = 0;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // ---------------- SCORE REGISTRATION ----------------

    public void AddMixScore(int amount)
    {
        mixScore += amount;
        RecalculateFinalScore();
        Debug.Log($"[TotalScoreManager] Mix Score Added: +{amount} (Total Mix: {mixScore})");
    }

    public void AddCutScore(int amount)
    {
        cutScore += amount;
        RecalculateFinalScore();
        Debug.Log($"[TotalScoreManager] Cut Score Added: +{amount} (Total Cut: {cutScore})");
    }

    public void AddSimmerScore(int amount)
    {
        simmerScore += amount;
        RecalculateFinalScore();
        Debug.Log($"[TotalScoreManager] Simmer Score Added: +{amount} (Total Simmer: {simmerScore})");
    }

    // ---------------- FINAL CALCULATION ----------------

    private void RecalculateFinalScore()
    {
        FinalScore = mixScore + cutScore + simmerScore;
    }

    public void CalculateFinalScore(string recipeName, RecipeSO recipe)
    {
        lastRecipe = recipe;

        if (recipe != null)
            targetScore = recipe.targetScore;

        RecalculateFinalScore();

        Debug.Log($"[TotalScoreManager] Final Score for {recipe.recipeName}: {FinalScore} / Target {targetScore}");

        if (completeDishPanel != null)
            completeDishPanel.SetActive(true);
    }

    // ---------------- SCOREBOARD DISPLAY ----------------

    public void ShowScoreBoard()
    {
        if (completeDishPanel != null) completeDishPanel.SetActive(false);
        if (scoreDashBoardPanel != null) scoreDashBoardPanel.SetActive(true);

        RecalculateFinalScore();
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
        int middleScore = Mathf.CeilToInt(targetScore * 0.5f);

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

    // ---------------- CONTINUE BUTTON ----------------

    public void OnContinueAndReturnToMap()
    {
        RecalculateFinalScore();
        int stars = CalculateStarCount();

        // Save your normal level results (for any other UI tracking)
        LevelResultSaver.SaveResult(currentLevelIndex, FinalScore, stars);

        // ✅ Save for IndicatorController unlock logic
        IndicatorController.SaveLevelProgress(currentLevelIndex, FinalScore, targetScore);

        Debug.Log($"[TotalScoreManager] Saved Level {currentLevelIndex} → Score: {FinalScore}, Stars: {stars}");

        ResetScores();
        SceneManager.LoadScene(mapSceneName);
    }

    // ---------------- RESET ----------------

    public void ResetScores()
    {
        mixScore = 0;
        cutScore = 0;
        simmerScore = 0;
        FinalScore = 0;
        lastRecipe = null;
        lastStarCount = 0;

        if (completeDishPanel != null) completeDishPanel.SetActive(false);
        if (scoreDashBoardPanel != null) scoreDashBoardPanel.SetActive(false);
    }
}
