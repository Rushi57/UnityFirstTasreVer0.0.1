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
               string.Equals(step.actionName?.Trim(), action?.Trim(), System.StringComparison.OrdinalIgnoreCase);
    }

    public void NextStep()
    {
        currentStepIndex++;
        if (currentStepIndex >= currentRecipe.steps.Count)
            Debug.Log("Recipe Completed!");
        else
            Debug.Log($"Step advanced → expecting: {GetExpectedStep()}");
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
            NextStep();
            return true;
        }
        WrongAttempt();
        return false;
    }

    public void TryMix()
    {
        if (IsCorrectAction("Mix"))
        {
            Debug.Log("Correct action: Mix");
            NextStep();
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
}
