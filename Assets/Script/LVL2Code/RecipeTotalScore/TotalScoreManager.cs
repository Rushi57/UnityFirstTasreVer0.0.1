using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TotalScoreManager : MonoBehaviour
{
    public static TotalScoreManager Instance;

    private int mixScore;
    private int cutScore;
    private int simmerScore;

    [Header("Panels")]
    [SerializeField] private GameObject completeDishPanel;   // Congrats Panel
    [SerializeField] private GameObject scoreDashBoardPanel; // Score Panel

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI mixingText;
    [SerializeField] private TextMeshProUGUI cuttingText;
    [SerializeField] private TextMeshProUGUI simmerText;
    [SerializeField] private TextMeshProUGUI totalScoreText;

    [Header("Dish UI Reference")]
    [SerializeField] private Image dishImageDisplay;
    [SerializeField] private TextMeshProUGUI dishNameText;

    [Header("Star References")]
    [SerializeField] private Image[] stars;
    [SerializeField] private Color starFilledColor = Color.yellow;
    [SerializeField] private Color starEmptyColor = Color.gray;

    public int starsEarned = 0;
    public int FinalScore { get; private set; }

    private RecipeSO lastRecipe; // keep track of completed recipe

    private void Awake()
    {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else Destroy(gameObject);
    }

    // ---------------------- SCORING ----------------------
    public void AddMixScore(int amount) => mixScore += amount;
    public void AddCutScore(int amount) => cutScore += amount;
    public void AddSimmerScore(int amount) => simmerScore += amount;

    // Called once when recipe is done
    // NOTE: now accepts only the RecipeSO; targetScore is read from the RecipeSO
    public void CalculateFinalScore(RecipeSO recipe)
    {
        if (!CookingStepManager.Instance.IsRecipeCompleted()) return;

        FinalScore = mixScore + cutScore + simmerScore;
        lastRecipe = recipe;

        Debug.Log($"📊 Mix:{mixScore}, Cut:{cutScore}, Simmer:{simmerScore}");
        Debug.Log($"🏆 Total Score: {FinalScore}");
        Debug.Log($"✅ Recipe {recipe.recipeName} Completed!");

        // compute stars using recipe.targetScore (fallback if <=0)
        int t = Mathf.Max(1, recipe.targetScore);
        starsEarned = CalculateStars(FinalScore, t);

        // save best stars for this recipe (persisted)
        SaveStarsForRecipe(recipe, starsEarned);

        if (completeDishPanel != null)
            completeDishPanel.SetActive(true);
        else
            Debug.LogWarning("⚠️ CompleteDishPanel not assigned!");
    }

    private int CalculateStars(int score, int targetScore)
    {
        if (score >= targetScore) return 3;              // Perfect
        else if (score >= Mathf.CeilToInt(targetScore * 0.7f)) return 2;  // Good
        else if (score >= Mathf.CeilToInt(targetScore * 0.4f)) return 1;  // Basic
        return 0;                                        // Fail
    }

    // ---------------------- PANEL FLOW ----------------------
    public void ShowScoreBoard()
    {
        if (completeDishPanel != null) completeDishPanel.SetActive(false);
        if (scoreDashBoardPanel != null) scoreDashBoardPanel.SetActive(true);

        UpdateScoreUI();
        UpdateStars();
        UpdateDishInfo();
    }

    private void UpdateScoreUI()
    {
        if (mixingText != null) mixingText.text = mixScore.ToString();
        if (cuttingText != null) cuttingText.text = cutScore.ToString();
        if (simmerText != null) simmerText.text = simmerScore.ToString();
        if (totalScoreText != null) totalScoreText.text = FinalScore.ToString();
    }

    private void UpdateStars()
    {
        for (int i = 0; i < stars.Length; i++)
        {
            if (stars[i] != null)
                stars[i].color = (i < starsEarned) ? starFilledColor : starEmptyColor;
        }
    }

    private void UpdateDishInfo()
    {
        if (lastRecipe != null)
        {
            if (dishImageDisplay != null && lastRecipe.recipeImage != null)
                dishImageDisplay.sprite = lastRecipe.recipeImage;

            if (dishNameText != null)
                dishNameText.text = lastRecipe.recipeName;
        }
    }

    // ---------------------- RESET ----------------------
    public void ResetScores()
    {
        mixScore = 0;
        cutScore = 0;
        simmerScore = 0;
        FinalScore = 0;
        starsEarned = 0;
        lastRecipe = null;

        if (completeDishPanel != null) completeDishPanel.SetActive(false);
        if (scoreDashBoardPanel != null) scoreDashBoardPanel.SetActive(false);
    }

    // ---------------------- PERSISTED STAR SAVE ----------------------
    private string GetRecipeKey(RecipeSO recipe)
    {
        string id = !string.IsNullOrEmpty(recipe.recipeID) ? recipe.recipeID : recipe.recipeName;
        return $"Stars_{id}";
    }

    private void SaveStarsForRecipe(RecipeSO recipe, int stars)
    {
        string key = GetRecipeKey(recipe);
        int prev = PlayerPrefs.GetInt(key, 0);
        if (stars > prev)
        {
            PlayerPrefs.SetInt(key, stars);
            PlayerPrefs.Save();
        }
        Debug.Log($"💾 Saved {stars} star(s) for {key}");
    }

    public int LoadStarsForRecipe(RecipeSO recipe)
    {
        string key = GetRecipeKey(recipe);
        return PlayerPrefs.GetInt(key, 0);
    }
}
