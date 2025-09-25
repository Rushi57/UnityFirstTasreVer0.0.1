using UnityEngine;

public class CookingStepManager : MonoBehaviour
{
    public static CookingStepManager Instance;

    [Header("Current Recipe")]
    public RecipeSO currentRecipe;
    public int currentStepIndex = 0; // tracks progress in recipe

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    /// <summary>
    /// Checks if the dropped item matches the current recipe step.
    /// </summary>
    public bool IsCorrectItem(ItemSO item)
    {
        if (currentRecipe == null) return false;
        if (currentStepIndex >= currentRecipe.steps.Count) return false;

        return currentRecipe.steps[currentStepIndex].itemID == item.itemID;
    }

    /// <summary>
    /// Advances to the next step in the recipe.
    /// </summary>
    public void NextStep()
    {
        currentStepIndex++;

        if (currentStepIndex >= currentRecipe.steps.Count)
        {
            Debug.Log("🎉 Recipe Completed!");
        }
        else
        {
            Debug.Log($"✅ Step advanced. Now expecting: {currentRecipe.steps[currentStepIndex].itemName}");
        }
    }

    /// <summary>
    /// Called when the wrong item is dropped.
    /// </summary>
    public void WrongAttempt()
    {
        Debug.Log("❌ Wrong item dropped!");
    }
}
