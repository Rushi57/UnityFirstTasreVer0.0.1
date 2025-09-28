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
    public bool canDrag = true;   // default true for inventory, set false when spawning on table
    
    private Draggable draggable;

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

        // 🚫 disable drag when spawned on table
        if (draggable != null)
            draggable.enabled = canDrag;
    }

    public void SetCutVersion()
    {
        if (itemSO != null && itemSO.choppedSprite != null)
            image.sprite = itemSO.choppedSprite;
    }

    // 👆 This makes prefab clickable
    public void OnPointerClick(PointerEventData eventData)
    {
        // Only allow chopping if this is an Ingredient
        if (itemSO == null || itemSO.itemType != ItemType.Ingredient)
        {
            Debug.Log($"⛔ {gameObject.name} is a Utility, skipping cutting.");
            return;
        }

        CuttingMechanic cuttingMechanic = FindObjectOfType<CuttingMechanic>();
        if (cuttingMechanic != null)
        {
            cuttingMechanic.StartCutting(image.sprite, this); // ✅ pass sprite + ItemData
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

    public void SetupZones(RectTransform red,  RectTransform yellow, RectTransform green)
    {
        float totalWidth = ((RectTransform)red.parent).rect.width;

        float redWidth = totalWidth = 0.6f;
        float yellowWidth = totalWidth = 0.3f;
        float greenWidth = totalWidth = 0.1f;

        //position Size
        red.sizeDelta = new Vector2(redWidth, red.sizeDelta.y);
        yellow.sizeDelta = new Vector2(yellowWidth, yellow.sizeDelta.y);
        green.sizeDelta = new Vector2(greenWidth, green.sizeDelta.y);

        // Move into place
        red.anchoredPosition = new Vector2(redWidth / 2, 0);
        yellow.anchoredPosition = new Vector2(redWidth + yellowWidth / 2, 0);
        green.anchoredPosition = new Vector2(redWidth + yellowWidth + greenWidth / 2, 0);
    }
}
