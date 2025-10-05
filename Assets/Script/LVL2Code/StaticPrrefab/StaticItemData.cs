using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class StaticItemData : MonoBehaviour, IPointerDownHandler
{
    [Header("Item Info")]
    public ItemSO itemSO; // assign in Inspector

    [Header("Optional: Spawn on Click")]
    public GameObject spawnPrefab; // PepperCornBits prefab
    public ItemSO spawnItemSO;     // PepperCornBits ItemSO data

    private Image image;
    private static GameObject activeSpawnedItem; // 👈 only allow one at a time

    void Awake()
    {
        image = GetComponent<Image>();
    }

    void Start()
    {
        if (itemSO != null && itemSO.itemSprite != null)
        {
            image.sprite = itemSO.itemSprite;
        }
    }

    public void SetupItem(ItemSO data)
    {
        itemSO = data;

        if (data != null && data.itemSprite != null)
            image.sprite = data.itemSprite;

        gameObject.name = data != null ? data.itemName : gameObject.name;
    }

    public void SetSprite(Sprite newSprite)
    {
        if (image != null && newSprite != null)
            image.sprite = newSprite;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // ✅ if a spawned item already exists, don't spawn again
        if (activeSpawnedItem != null)
        {
            Debug.Log("⚠️ Another spawned item already exists, skipping spawn.");
            return;
        }

        if (spawnPrefab == null || spawnItemSO == null)
        {
            Debug.Log($"ℹ️ {gameObject.name} has no spawn prefab or ItemSO assigned.");
            return;
        }

        // ✅ find main canvas
        Canvas mainCanvas = FindObjectOfType<Canvas>();
        if (mainCanvas == null)
        {
            Debug.LogWarning("⚠️ No Canvas found in scene!");
            return;
        }

        // ✅ spawn prefab and set as active reference
        GameObject spawned = Instantiate(spawnPrefab, mainCanvas.transform);
        activeSpawnedItem = spawned; // store so we know something is spawned
        spawned.name = spawnItemSO.itemName + "_Spawned";

        // ✅ setup visual + data
        Image img = spawned.GetComponent<Image>();
        if (img != null && spawnItemSO.itemSprite != null)
            img.sprite = spawnItemSO.itemSprite;

        RectTransform rect = spawned.GetComponent<RectTransform>();
        if (rect != null)
        {
            rect.localScale = Vector3.one;
            rect.sizeDelta = new Vector2(60, 60);
        }

        // ✅ ensure item can be dragged
        ItemData itemData = spawned.GetComponent<ItemData>();
        if (itemData != null)
        {
            itemData.itemSO = spawnItemSO;
            itemData.canDrag = true;
        }

        // ✅ ensure draggable + canvas group
        Draggable draggable = spawned.GetComponent<Draggable>();
        if (draggable == null)
            draggable = spawned.AddComponent<Draggable>();

        CanvasGroup group = spawned.GetComponent<CanvasGroup>();
        if (group == null)
            group = spawned.AddComponent<CanvasGroup>();

        // ✅ set spawn position under cursor
        Vector2 spawnPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            mainCanvas.transform as RectTransform,
            eventData.position,
            mainCanvas.worldCamera,
            out spawnPos
        );
        rect.anchoredPosition = spawnPos;

        // ✅ bring to front (fixes behind-layer issue)
        spawned.transform.SetAsLastSibling();

        Debug.Log($"✅ Spawned {spawnItemSO.itemID} from {itemSO.itemName}");

        // ✅ start dragging immediately
        ExecuteEvents.Execute<IDragHandler>(spawned, eventData, (x, y) => x.OnDrag(eventData));

        // 🧹 cleanup: if object destroyed elsewhere, clear reference automatically
        SpawnCleanupWatcher watcher = spawned.AddComponent<SpawnCleanupWatcher>();
        watcher.onDestroyed = () => activeSpawnedItem = null;
    }

    // Helper class to auto-reset activeSpawnedItem when destroyed
    private class SpawnCleanupWatcher : MonoBehaviour
    {
        public System.Action onDestroyed;
        void OnDestroy() => onDestroyed?.Invoke();
    }
}
