using UnityEngine;

public class SpawnToTable : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform tableArea;   // Parent where items will spawn
    [SerializeField] private GameObject slotPrefab; // Prefab to spawn (must have ItemData or StaticItemData)

    [Header("Item Data")]
    [SerializeField] private ItemSO itemSO; // The item this button will spawn

    private void Start()
    {
        if (tableArea == null)
            Debug.LogError("❌ Missing Table Area reference!");
        if (slotPrefab == null)
            Debug.LogError("❌ Missing Slot Prefab reference!");
        if (itemSO == null)
            Debug.LogError("❌ Missing ItemSO reference!");
    }

    // Call this from the Button OnClick()
    public void SpawnItem()
    {
        if (tableArea == null || slotPrefab == null || itemSO == null)
        {
            Debug.LogError("⚠️ Missing reference in SpawnToTable!");
            return;
        }

        // ✅ Check if the item already exists inside the table
        bool alreadySpawned = false;
        foreach (Transform child in tableArea)
        {
            StaticItemData staticData = child.GetComponent<StaticItemData>();
            ItemData itemData = child.GetComponent<ItemData>();

            if (staticData != null && staticData.itemSO == itemSO)
            {
                alreadySpawned = true;
                break;
            }

            if (itemData != null && itemData.itemSO == itemSO)
            {
                alreadySpawned = true;
                break;
            }
        }

        if (alreadySpawned)
        {
            Debug.Log($"⚠️ {itemSO.itemName} already on table — skipping spawn.");
            return;
        }

        // ✅ Spawn prefab inside TableArea (auto layout handles position)
        GameObject newSlot = Instantiate(slotPrefab, tableArea);

        // Assign ItemSO data
        StaticItemData staticItem = newSlot.GetComponent<StaticItemData>();
        ItemData draggableItem = newSlot.GetComponent<ItemData>();

        if (staticItem != null)
        {
            staticItem.SetupItem(itemSO);
            Debug.Log($"✅ Spawned STATIC item: {itemSO.itemName}");
        }
        else if (draggableItem != null)
        {
            draggableItem.SetupItem(itemSO);
            Debug.Log($"✅ Spawned DRAGGABLE item: {itemSO.itemName}");
        }
        else
        {
            Debug.LogWarning("⚠️ Spawned prefab has no ItemData or StaticItemData!");
        }
    }
}
