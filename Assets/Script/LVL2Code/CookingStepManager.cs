using UnityEngine;

public class CookingStepManager : MonoBehaviour
{
    public static CookingStepManager Instance;

    [Header("Current Recipe")]
    public RecipeSO currentRecipe;
    public int currentStepIndex = 0;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        if (GameSession.SelectedRecipe != null)
        {
            currentRecipe = GameSession.SelectedRecipe;
            Debug.Log("Loaded recipe from GameSession: " + currentRecipe.recipeName);
        }
        else
        {
            Debug.Log("No recipe selected via GameSession — using inspector currentRecipe");
        }
    }

    private bool HasCurrentStep()
    {
        return currentRecipe != null && currentStepIndex < currentRecipe.steps.Count;
    }

    public bool IsCorrectItem(ItemSO item)
    {
        if (!HasCurrentStep()) return false;
        var step = currentRecipe.steps[currentStepIndex];
        return step.stepType == StepType.Ingredient &&
               step.ingredient != null &&
               step.ingredient.itemID == item.itemID;
    }

    public bool IsCorrectAction(string action)
    {
        if (!HasCurrentStep()) return false;
        var step = currentRecipe.steps[currentStepIndex];
        return step.stepType == StepType.Action &&
               string.Equals(step.actionName?.Trim(), action?.Trim(),
               System.StringComparison.OrdinalIgnoreCase);
    }

    // ✅ Simplified NextStep — always shows next step immediately
    public void NextStep()
    {
        currentStepIndex++;

        if (currentStepIndex >= currentRecipe.steps.Count)
        {
            TotalScoreManager.Instance.CalculateFinalScore("Level", currentRecipe);
            return;
        }

        // ✅ Always show the next step message
        Debug.Log($"Next step: {GetExpectedStep()}");
    }

    public void WrongAttempt()
    {
        DebugMessageManager.Instance.ShowMessage("Wrong item or action for this step!");
    }

    public bool OnActionPerformed(string actionName)
    {
        if (IsCorrectAction(actionName))
        {
            Debug.Log("Correct action: " + actionName);
            NextStep(); // ✅ Always show next step message immediately
            return true;
        }

        WrongAttempt();
        return false;
    }

    public void TryMix()
    {
        if (IsCorrectAction("mix"))
        {
            Debug.Log("Correct action: mix");
            NextStep();
        }
        else
        {
            WrongAttempt();
        }
    }

    public void TrySimmer()
    {
        if (IsCorrectAction("simmer"))
        {
            Debug.Log("Correct action: simmer");
            
        }
        else
        {
            WrongAttempt();
        }
    }

    public void TryCut()
    {
        if (IsCorrectAction("cut"))
        {
            Debug.Log("Correct action: cut");
            NextStep();
        }
        else
        {
            WrongAttempt();
        }
    }
    public void TryBoil()
    {
        if (IsCorrectAction("boil"))
        {
            Debug.Log("Correct action: boil");
            
        }
        else
        {
            WrongAttempt();
        }
    }

    public string GetExpectedStep()
    {
        if (!HasCurrentStep()) return null;
        var step = currentRecipe.steps[currentStepIndex];
        return step.stepType == StepType.Action
            ? step.actionName
            : step.ingredient != null ? step.ingredient.itemName : null;
    }

    public bool IsRecipeCompleted()
    {
        return currentRecipe != null && currentStepIndex >= currentRecipe.steps.Count;
    }
}
