using UnityEngine;

public enum ItemType
{
    Utility,     // Example: Pan, Pot, Knife
    Ingredient   // Example: Pork, Soy Sauce, Vinegar
}

[CreateAssetMenu(fileName = "NewItem", menuName = "Cooking/Item")]
public class ItemSO : ScriptableObject
{
    [Header("Identification")]
    public string itemID;     // unique ID, e.g. "pan_001", "pork_001"
    public string itemName;   // display name, e.g. "Pan", "Pork Meat"
    public ItemType itemType;

    [Header("Visuals")]
    public Sprite itemSprite; // icon or image used in UI
    public Sprite choppedSprite; // icon for the CutSprite
}
