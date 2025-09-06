using UnityEngine;

public class CookbookManager : MonoBehaviour
{
    public RecipieSO currentRecipies;
    private int currentStep = 0;

    public void StartRecipie(RecipieSO recipie)
    {
        currentRecipies = recipie;
        currentStep = 0;
    }

    public bool TryAddIngredient(string ingredientName)
    {
        if (currentRecipies == null) return false;
        
        if (currentRecipies.steps[currentStep] == ingredientName)
        {
            //Debug
            currentStep++;

            if(currentStep >= currentRecipies.steps.Count)
            {
                return true;
            }
            return true;

        }
        else
        {
            return false;
        }
    
    }
    public string GetNextIngredients()
    {
        if (currentRecipies == null || currentStep >= currentRecipies.steps.Count)
            return "None";
        return currentRecipies.steps[currentStep];
    }
}
