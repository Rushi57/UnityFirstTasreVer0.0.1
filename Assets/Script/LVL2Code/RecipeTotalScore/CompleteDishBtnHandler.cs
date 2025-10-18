using UnityEngine;

public class CompleteDishBtnHandler : MonoBehaviour
{
    public RecipeSO recipeToShow;               // Assign in Inspector
    public CongratsDishPanel congratsPanel;     // Drag your panel here

    public void OnButtonClick()
    {
        if (congratsPanel != null && recipeToShow != null)
        {
            congratsPanel.ShowCongrats(recipeToShow);
        }
        else
        {
            Debug.LogWarning("CongratsDishPanel or RecipeSO not assigned!");
        }
    }
}
