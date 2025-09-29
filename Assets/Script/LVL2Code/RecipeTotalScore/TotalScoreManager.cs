using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TotalScoreManager : MonoBehaviour
{
    public static TotalScoreManager Instance;

    [Header("UI References")]
    public TextMeshProUGUI mixingScoreText;
    public TextMeshProUGUI choppingScoreText;
    public TextMeshProUGUI simmerScoreText;
    public TextMeshProUGUI totalScoreText;

    [Header("Recipe Info UI")]
    public TextMeshProUGUI recipeNameText;
    public Image recipeImage;

    [Header("Stars")]
    public Image[] stars; // assign 3 star UI Images in inspector
    public Color filledStarColor = Color.yellow;
    public Color emptyStarColor = Color.gray;

    private int finalScore;
    private int targetScore;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    /// <summary>
    /// Calculates final score and updates completion panel
    /// </summary>
    public void CalculateFinalScore(RecipeSO recipe)
    {
        int mixScore = MixingScoreManager.Instance != null ? MixingScoreManager.Instance.totalMixScore : 0;
        int cutScore = CutScoreManager.Instance != null ? CutScoreManager.Instance.choppedTotalscore : 0;
        int simmerScore = SimmerScoreManager.Instance != null ? SimmerScoreManager.Instance.simmerTotalScore : 0;

        finalScore = mixScore + cutScore + simmerScore;

        // Max possible score = steps × 3 points (since 3 = best rating per step)
        targetScore = recipe.steps.Count * 3;

        // Update UI
        if (mixingScoreText != null) mixingScoreText.text = $"Mixing Score: {mixScore}";
        if (choppingScoreText != null) choppingScoreText.text = $"Chopping Score: {cutScore}";
        if (simmerScoreText != null) simmerScoreText.text = $"Simmer Score: {simmerScore}";
        if (totalScoreText != null) totalScoreText.text = $"Total Score: {finalScore}/{targetScore}";

        if (recipeNameText != null) recipeNameText.text = recipe.recipeName;
        if (recipeImage != null && recipe.recipeImage != null) recipeImage.sprite = recipe.recipeImage;

        UpdateStars();
    }

    private void UpdateStars()
    {
        if (stars == null || stars.Length < 3) return;

        // Reset all stars to empty
        foreach (var star in stars)
            star.color = emptyStarColor;

        if (finalScore >= targetScore)
        {
            // ⭐⭐⭐ Perfect!
            for (int i = 0; i < 3; i++) stars[i].color = filledStarColor;
        }
        else if (finalScore >= targetScore * 0.7f)
        {
            // ⭐⭐
            for (int i = 0; i < 2; i++) stars[i].color = filledStarColor;
        }
        else if (finalScore > 0)
        {
            // ⭐
            stars[0].color = filledStarColor;
        }
    }
}
