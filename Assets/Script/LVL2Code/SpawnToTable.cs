using UnityEngine;

public class SpawnToTable : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform tableArea;   // Parent where items will spawn
    [SerializeField] private GameObject slotPrefab; // Prefab to spawn (must have ItemData + Image)

    [Header("Item Data")]
    [SerializeField] private ItemSO itemSO; // The item this button will spawn

    // Call this from the Button OnClick()
    public void SpawnItem()
    {
        if (tableArea == null || slotPrefab == null || itemSO == null)
        {
            Debug.LogError("Missing reference in SpawnToTable!");
            return;
        }

        // Spawn prefab inside TableArea
        GameObject newSlot = Instantiate(slotPrefab, tableArea);

        StaticItemData staticData = newSlot.GetComponent<StaticItemData>();
        ItemData itemData = newSlot.GetComponent<ItemData>();

        if (staticData != null)
        {
            staticData.SetupItem(itemSO);
            Debug.Log($"✅ Spawned STATIC item: {itemSO.itemName}");
        }
        else if (itemData != null)
        {
            itemData.SetupItem(itemSO);
            Debug.Log($"✅ Spawned DRAGGABLE item: {itemSO.itemName}");
        }
        else
        {
            Debug.LogWarning("⚠️ Spawned prefab has no ItemData or StaticItemData!");
        }
    }
}
