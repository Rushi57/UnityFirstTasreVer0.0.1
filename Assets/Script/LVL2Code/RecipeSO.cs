using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewRecipe", menuName = "Cooking/Recipe")]
public class RecipeSO : ScriptableObject
{
    [Header("Recipe Info")]
    public string recipeName;  // e.g. "Pork Adobo"
    public Sprite recipeImage; // optional preview

    [Header("Steps")]
    public List<ItemSO> steps = new List<ItemSO>();
    // Order matters: first Pan, then Pork, then Soy Sauce, etc.
}
