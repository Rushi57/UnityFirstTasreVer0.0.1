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
            Debug.LogError("⚠️ Missing reference in SpawnToTable!");
            return;
        }

        // Spawn prefab inside TableArea
        GameObject newSlot = Instantiate(slotPrefab, tableArea);

        // Setup visuals + data
        ItemData data = newSlot.GetComponent<ItemData>();
        if (data != null)
        {
            data.SetupItem(itemSO); // load sprite + type from ItemSO
        }
        else
        {
            Debug.LogWarning("⚠️ Spawned prefab has no ItemData!");
        }

        Debug.Log($"✅ Spawned {itemSO.itemName} in TableArea.");
    }
}
