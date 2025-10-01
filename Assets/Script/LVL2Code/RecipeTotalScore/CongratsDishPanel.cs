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

    private RecipeSO currentRecipe;

    // Call this when a recipe is completed
    public void ShowCongrats(RecipeSO recipe)
    {
        currentRecipe = recipe;

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
