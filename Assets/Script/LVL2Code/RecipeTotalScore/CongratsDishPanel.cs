using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CongratsDishPanel : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI congratsText;
    public Image recipeImageDisplay;

    [Header("Next Panel Reference")]
    public GameObject scoreDashBoardPanel; // drag your ScoreDashboardPanel here

    [Header("Recipe Info (Optional Manual Assignment)")]
    [Tooltip("Assign a RecipeSO here to display its name and image automatically.")]
    public RecipeSO recipeToDisplay;

    private void Start()
    {
        // ✅ If a recipe is assigned in the Inspector, display it immediately
        if (recipeToDisplay != null)
        {
            ShowCongrats(recipeToDisplay);
        }
    }

    // ✅ You can still call this from other scripts if needed
    public void ShowCongrats(RecipeSO recipe)
    {
        recipeToDisplay = recipe;

        if (congratsText != null)
            congratsText.text = $"🎉 Congratulations! You completed: <b>{recipe.recipeName}</b> 🎉";

        if (recipeImageDisplay != null && recipe.recipeImage != null)
            recipeImageDisplay.sprite = recipe.recipeImage;

        gameObject.SetActive(true);
    }

    public void HideCongrats()
    {
        gameObject.SetActive(false);
    }

    // 👇 Call this on Button (OnClick event) or detect tap anywhere
    public void OnClickContinue()
    {
        HideCongrats();

        if (scoreDashBoardPanel != null)
        {
            scoreDashBoardPanel.SetActive(true);

            // also tell TotalScoreManager to update UI
            if (TotalScoreManager.Instance != null)
            {
                TotalScoreManager.Instance.ShowScoreBoard();
            }
        }
    }
}
