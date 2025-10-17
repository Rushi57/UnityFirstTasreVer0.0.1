using System.Collections.Generic;
using UnityEngine;

public enum StepType { Ingredient, Action }

[System.Serializable]

public class RecipeStep
{
    public StepType stepType;

    // Ingredient step
    public ItemSO ingredient;

    // Action step (like Oil, Vinegar, Soy)
    public string actionName;
    public string Action;
}

[CreateAssetMenu(fileName = "NewRecipe", menuName = "Cooking/Recipe")]
public class RecipeSO : ScriptableObject
{
    [Header("Recipe Info")]
    public string recipeID;     // Unique identifier (e.g. "adobo_001")
    public string recipeName;
    public Sprite recipeImage;
    [Header("Scoring")]
    public int targetScore;
    [Header("Steps")]
    public List<RecipeStep> steps = new List<RecipeStep>();

}
