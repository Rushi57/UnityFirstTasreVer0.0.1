using UnityEngine;

public class SpawnToTable : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform tableArea;   // Parent where items will spawn
    [SerializeField] private GameObject slotPrefab; // Prefab to spawn (must have ItemData + Image)

    [Header("Item Data")]
    [SerializeField] private ItemSO itemSO; // The item this button will spawn

    [Header("Spawn Layout Settings")]
    [SerializeField] private int columns = 4;       // How many per row
    [SerializeField] private float cellWidth = 120f;
    [SerializeField] private float cellHeight = 120f;
    [SerializeField] private Vector2 startOffset = new Vector2(50f, -50f);

    private int spawnCount = 0;

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
        RectTransform rect = newSlot.GetComponent<RectTransform>();

        // Calculate grid position (no overlap)
        int row = spawnCount / columns;
        int col = spawnCount % columns;

        Vector2 spawnPos = new Vector2(
            startOffset.x + (col * cellWidth),
            startOffset.y - (row * cellHeight)
        );

        rect.anchoredPosition = spawnPos;

        // Assign ItemSO data
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

        // Increment counter
        spawnCount++;
    }
}
