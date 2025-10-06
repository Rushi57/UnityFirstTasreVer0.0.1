using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class CookBookIngredient
{
    public ItemSO itemSO;                     // Reference to existing ItemSO
    [TextArea(2, 4)] public string description; // Unique description for cookbook
}

[CreateAssetMenu(fileName = "NewCookBook", menuName = "CookBook/New Dish")]
public class CookBookSO : ScriptableObject
{
    [Header("Dish Info")]
    public string dishName;
    public string region;
    [TextArea(3, 6)] public string dishDescription;
    public Sprite dishImage;

    [Header("Ingredients")]
    public List<CookBookIngredient> ingredients = new List<CookBookIngredient>();
}
