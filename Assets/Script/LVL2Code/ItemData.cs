using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Image))]
public class ItemData : MonoBehaviour, IPointerClickHandler
{
    public ItemSO itemSO;
    private Image image;

    [HideInInspector] public Transform originalParent;
    [HideInInspector] public Vector2 originalAnchoredPosition;

    [Header("Interaction")]
    public bool canDrag = true;   // default true for inventory
    private Draggable draggable;

    // ✅ new flag to block re-cutting
    private bool hasBeenCut = false;

    void Awake()
    {
        image = GetComponent<Image>();
        draggable = GetComponent<Draggable>();
    }

    public void SetupItem(ItemSO data)
    {
        itemSO = data;
        if (image == null) image = GetComponent<Image>();

        if (data != null && data.itemSprite != null)
            image.sprite = data.itemSprite;

        gameObject.name = data != null ? data.itemName : gameObject.name;

        if (draggable != null)
            draggable.enabled = canDrag;

        hasBeenCut = false; // reset when new item setup
    }

    public void SetCutVersion()
    {
        if (itemSO != null && itemSO.choppedSprite != null)
            image.sprite = itemSO.choppedSprite;

        hasBeenCut = true; // ✅ mark as already cut
    }

    // 👆 This makes prefab clickable
    public void OnPointerClick(PointerEventData eventData)
    {
        // 🚫 if already cut, don’t open cutting mechanic again
        if (hasBeenCut)
        {
            Debug.Log($"⛔ {gameObject.name} is already cut. Skipping CuttingMechanic.");
            return;
        }

        // Only allow chopping if this is an Ingredient
        if (itemSO == null || itemSO.itemType != ItemType.Ingredient)
        {
            Debug.Log($"⛔ {gameObject.name} is a Utility, skipping cutting.");
            return;
        }

        CuttingMechanic cuttingMechanic = FindObjectOfType<CuttingMechanic>();
        if (cuttingMechanic != null)
        {
            cuttingMechanic.StartCutting(image.sprite, this);
        }
        else
        {
            Debug.LogWarning("⚠️ No CuttingMechanic in scene!");
        }
    }

    public void EnableDrag()
    {
        canDrag = true;
        if (draggable != null)
            draggable.enabled = true;
    }

    public void SetupZones(RectTransform red, RectTransform yellow, RectTransform green)
    {
        float totalWidth = ((RectTransform)red.parent).rect.width;

        float redWidth = totalWidth * 0.6f;
        float yellowWidth = totalWidth * 0.3f;
        float greenWidth = totalWidth * 0.1f;

        // Position & size
        red.sizeDelta = new Vector2(redWidth, red.sizeDelta.y);
        yellow.sizeDelta = new Vector2(yellowWidth, yellow.sizeDelta.y);
        green.sizeDelta = new Vector2(greenWidth, green.sizeDelta.y);

        red.anchoredPosition = new Vector2(redWidth / 2, 0);
        yellow.anchoredPosition = new Vector2(redWidth + yellowWidth / 2, 0);
        green.anchoredPosition = new Vector2(redWidth + yellowWidth + greenWidth / 2, 0);
    }
}
