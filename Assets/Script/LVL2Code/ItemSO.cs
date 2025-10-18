using UnityEngine;

public enum ItemType
{
    Utility,     // Example: Pan, Pot, Knife
    Ingredient,   // Example: Pork, Soy Sauce, Vinegar
    Action
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

    [Header("Spawn Chain (Optional)")]
    public GameObject spawnPrefab;     // prefab to spawn (e.g., PepperCornBitsPrefab)
    public ItemSO spawnItemSO;         // item data for that spawned prefab    

    [Header("Audio")]
    public AudioClip dropSFX;
}
