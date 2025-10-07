using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCookBookSO", menuName = "CookBook/CookBookSO")]
public class CookBookSO : ScriptableObject
{
    [Header("Dish Info")]
    public string dishName;
    public string region;
    [TextArea(2, 5)] public string dishDescription;
    public Sprite dishImage;

    [Header("Ingredients")]
    public List<IngredientElement> ingredients = new List<IngredientElement>();
}

[System.Serializable]
public class IngredientElement
{
    public ItemSO itemSO;
    public string description;
}
