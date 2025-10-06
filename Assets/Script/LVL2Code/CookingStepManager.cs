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
               string.Equals(step.actionName?.Trim(), action?.Trim(), System.StringComparison.OrdinalIgnoreCase);
    }

    // ✅ Updated NextStep with suppressMessage behavior
    public void NextStep(bool suppressMessage = false)
    {
        currentStepIndex++;

        if (currentStepIndex >= currentRecipe.steps.Count)
        {
            TotalScoreManager.Instance.CalculateFinalScore("Level1", currentRecipe);
            return;
        }

        var nextStep = currentRecipe.steps[currentStepIndex];

        // 🧠 Suppress the message if caller asked (e.g. Mix/Simmer started)
        if (suppressMessage)
        {
            Debug.Log($"[CookingStepManager] Message suppressed for step: {GetExpectedStep()}");
            return;
        }

        // 🧠 Skip showing message if next step is a mini-game (Mix or Simmer)
        if (nextStep.stepType == StepType.Action &&
            (nextStep.actionName.Equals("Mix", System.StringComparison.OrdinalIgnoreCase) ||
             nextStep.actionName.Equals("Simmer", System.StringComparison.OrdinalIgnoreCase)))
        {
            Debug.Log($"[CookingStepManager] Suppressing message until {nextStep.actionName} mini-game finishes");
            return;
        }

        // ✅ Normal case — show next step message
        DebugMessageManager.Instance.ShowMessage($"Next step: {GetExpectedStep()}");
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

            // If the action is Mix or Simmer, suppress until mini-game completes
            if (actionName.Equals("Mix", System.StringComparison.OrdinalIgnoreCase) ||
                actionName.Equals("Simmer", System.StringComparison.OrdinalIgnoreCase))
            {
                NextStep(true); // ✅ Suppress next step message
            }
            else
            {
                NextStep(); // ✅ Show normal message
            }

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
            NextStep(true); // ✅ No "Next step" message yet — wait for mini-game to finish
        }
        else
        {
            WrongAttempt();
        }
    }

    public void TrySimmer()
    {
        if (IsCorrectAction("Simmer"))
        {
            Debug.Log("Correct action: Simmer");
            NextStep(true); // ✅ No "Next step" message yet — wait for mini-game to finish
        }
        else
        {
            WrongAttempt();
        }
    }

    public void TryCut()
    {
        if (IsCorrectAction("Cut"))
        {
            Debug.Log("Correct action: Cut");
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

    public bool IsRecipeCompleted()
    {
        return currentRecipe != null && currentStepIndex >= currentRecipe.steps.Count;
    }
}
