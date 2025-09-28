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

    public bool IsCorrectItem(ItemSO item)
    {
        if (currentRecipe == null) return false;
        if (currentStepIndex >= currentRecipe.steps.Count) return false;

        var step = currentRecipe.steps[currentStepIndex];

        if (step.stepType == StepType.Ingredient)
        {
            return step.ingredient != null && step.ingredient.itemID == item.itemID;
        }

        return false;
    }

    public bool IsCorrectAction(string action)
    {
        if (currentRecipe == null) return false;
        if (currentStepIndex >= currentRecipe.steps.Count) return false;

        var step = currentRecipe.steps[currentStepIndex];

        if (step.stepType == StepType.Action)
        {
            return step.actionName == action;
        }

        return false;
    }

    public void NextStep()
    {
        currentStepIndex++;

        if (currentStepIndex >= currentRecipe.steps.Count)
        {
            Debug.Log(" Recipe Completed!");
        }
        else
        {
            Debug.Log($"Step advanced. Now expecting next step: {currentStepIndex}");
        }
    }

    public void WrongAttempt()
    {
        DebugMessageManager.Instance.ShowMessage(" Wrong item or Ingredient is dropped!");
    }

    public void OnActionPerformed(string actionName)
    {
       if(currentRecipe == null) return;
       if(currentStepIndex >= currentRecipe.steps.Count) return;

       var step = currentRecipe.steps[currentStepIndex];

        if(step.stepType == StepType.Action && step.actionName == actionName)
        {
            Debug.Log("Correct action: " + actionName);
            NextStep();
        }
        else
        {
            WrongAttempt();
        }
    }
    public string GetExpectedStep()
    {
        if (currentStepIndex < currentRecipe.steps.Count)
        {
            var step = currentRecipe.steps[currentStepIndex];

            if (step.stepType == StepType.Action)
                return step.actionName;

            if (step.stepType == StepType.Ingredient && step.ingredient != null)
                return step.ingredient.itemName; // optional, if you want ingredient names too
        }
        return null;
    }



}
