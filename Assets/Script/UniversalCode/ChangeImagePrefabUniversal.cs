using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CookwareStateHandler : MonoBehaviour, IDropHandler
{
    [Header("UI References")]
    public GameObject mixingMechPanel;   // Mixing panel for stirring, etc.
    public GameObject simmerClockPanel;  // Simmer panel for timing

    [Header("Cookware Visuals")]
    public Image cookwareImage;          // UI image showing the cookware state
    public Sprite defaultSprite;         // Base sprite (empty cookware)

    [Header("Ingredient Step Sprites")]
    public Sprite[] stepSprites;         // Ordered per ingredient step

    [Header("Action Sprites (Condiments / Liquids)")]
    public Sprite oilSprite;
    public Sprite vinegarSprite;
    public Sprite soySprite;
    public Sprite waterSprite;
    public Sprite saltSprite;
    public Sprite pasteSprite;           // Added for paste actions

    private int stepIndex = 0;

    public void OnDrop(PointerEventData eventData)
    {
        var droppedItem = eventData.pointerDrag?.GetComponent<ItemData>();
        if (droppedItem == null) return;

        ItemSO item = droppedItem.itemSO;
        if (item == null) return;

        // --- INGREDIENT DROP ---
        if (item.itemType == ItemType.Ingredient)
        {
            if (CookingStepManager.Instance.IsCorrectItem(item))
            {
                UpdateCookwareIngredient();
                CookingStepManager.Instance.NextStep();

                Debug.Log("✅ Accepted ingredient: " + item.itemName);
                Destroy(eventData.pointerDrag.gameObject);
            }
            else
            {
                droppedItem.GetComponent<Draggable>()?.RevertToOriginalPosition();
                CookingStepManager.Instance.WrongAttempt();
            }
            return;
        }

        // --- MIX / STIR ACTION (e.g., using a Spatula) ---
        if (CookingStepManager.Instance.IsCorrectAction("Mix") &&
            item.itemName.Equals("Spatula", System.StringComparison.OrdinalIgnoreCase))
        {
            CookingStepManager.Instance.NextStep();
            mixingMechPanel?.SetActive(true);
            Destroy(eventData.pointerDrag.gameObject);
            return;
        }

        // --- SIMMER ACTION (e.g., using a Lid) ---
        if (CookingStepManager.Instance.IsCorrectAction("Simmer") &&
            (item.itemName.Equals("PanLid", System.StringComparison.OrdinalIgnoreCase) ||
             item.itemName.Equals("PotLid", System.StringComparison.OrdinalIgnoreCase)))
        {
            CookingStepManager.Instance.NextStep();
            simmerClockPanel?.SetActive(true);
            Destroy(eventData.pointerDrag.gameObject);
            return;
        }

        // --- CONDIMENT / ACTION DROP ---
        if (item.itemType == ItemType.Action)
        {
            string actionName = item.itemName.Trim().ToLower();
            if (CookingStepManager.Instance.OnActionPerformed(actionName))
            {
                UpdateCookwareWithAction(actionName);
                Debug.Log("✅ Accepted action: " + actionName);
                Destroy(eventData.pointerDrag.gameObject);
            }
            else
            {
                droppedItem.GetComponent<Draggable>()?.RevertToOriginalPosition();
            }
        }
    }

    public void UpdateCookwareIngredient()
    {
        if (stepSprites == null || stepSprites.Length == 0) return;

        if (stepIndex < stepSprites.Length)
        {
            cookwareImage.sprite = stepSprites[stepIndex];
            stepIndex++;
        }
        else
        {
            Debug.LogWarning("[CookwareStateHandler] No more ingredient sprites available.");
        }
    }

    public void UpdateCookwareWithAction(string action)
    {
        if (string.IsNullOrEmpty(action)) return;

        switch (action)
        {
            case "oil":
                if (oilSprite) cookwareImage.sprite = oilSprite;
                break;
            case "vinegar":
                if (vinegarSprite) cookwareImage.sprite = vinegarSprite;
                break;
            case "soy sauce":
                if (soySprite) cookwareImage.sprite = soySprite;
                break;
            case "water":
                if (waterSprite) cookwareImage.sprite = waterSprite;
                break;
            case "salt":
                if (saltSprite) cookwareImage.sprite = saltSprite;
                break;
            case "paste":
            case "tomato paste":
            case "garlic paste":
                if (pasteSprite) cookwareImage.sprite = pasteSprite;
                break;
            default:
                Debug.Log($"[CookwareStateHandler] No sprite assigned for action: {action}");
                break;
        }
    }

    public void ResetCookware()
    {
        cookwareImage.sprite = defaultSprite;
        stepIndex = 0;
    }
}
