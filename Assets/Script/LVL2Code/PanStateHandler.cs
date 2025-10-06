using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PanStateHandler : MonoBehaviour, IDropHandler
{
    [Header("UI Reference")]
    public GameObject mixingMechPanel;   // MixingPanel
    public GameObject simmerClockPanel;

    [Header("Pan Images")]
    public Image panImage;
    public Sprite defaultPanSprite;

    [Header("Ingredient Step Sprites")]
    public Sprite[] stepSprites; // ordered per ingredient or action step

    private int stepIndex = 0;

    public void OnDrop(PointerEventData eventData)
    {
        var droppedItem = eventData.pointerDrag?.GetComponent<ItemData>();
        if (droppedItem == null) return;

        // --- INGREDIENT DROP ---
        if (droppedItem.itemSO.itemType == ItemType.Ingredient)
        {
            if (CookingStepManager.Instance.IsCorrectItem(droppedItem.itemSO))
            {
                AdvanceStep();
                CookingStepManager.Instance.NextStep();

                Debug.Log("[PanStateHandler] Ingredient accepted: " + droppedItem.itemSO.itemName);
                Destroy(eventData.pointerDrag.gameObject);
            }
            else
            {
                droppedItem.GetComponent<Draggable>()?.RevertToOriginalPosition();
                CookingStepManager.Instance.WrongAttempt();
            }
        }

        // --- MIX / SPATULA STEP ---
        else if (CookingStepManager.Instance.IsCorrectAction("Mix") &&
                 droppedItem.itemSO.itemName.Equals("Spatula", System.StringComparison.OrdinalIgnoreCase))
        {
            CookingStepManager.Instance.NextStep();
            mixingMechPanel.SetActive(true);

            Debug.Log("[PanStateHandler] Mix action triggered.");
            Destroy(eventData.pointerDrag.gameObject);
        }

        // --- SIMMER / PAN LID STEP ---
        else if (CookingStepManager.Instance.IsCorrectAction("Simmer") &&
                 droppedItem.itemSO.itemName.Equals("PanLid", System.StringComparison.OrdinalIgnoreCase))
        {
            CookingStepManager.Instance.NextStep();
            simmerClockPanel.SetActive(true);

            Debug.Log("[PanStateHandler] Simmer action triggered.");
            Destroy(eventData.pointerDrag.gameObject);
        }

        // --- CONDIMENT / ACTION DROP ---
        else if (droppedItem.itemSO.itemType == ItemType.Action)
        {
            string actionName = droppedItem.itemSO.itemName.Trim();
            Debug.Log($"[PanStateHandler] Detected Action Drop: {actionName}");

            if (CookingStepManager.Instance.OnActionPerformed(actionName))
            {
                AdvanceStep(); // ✅ now it uses next step sprite instead of oil/vinegar sprite
                CookingStepManager.Instance.NextStep();

                Debug.Log($"[PanStateHandler] Correct action '{actionName}' — advanced to next sprite.");
                Destroy(eventData.pointerDrag.gameObject);
            }
            else
            {
                Debug.LogWarning($"[PanStateHandler] Action '{actionName}' not expected right now.");
                droppedItem.GetComponent<Draggable>()?.RevertToOriginalPosition();
            }
        }
    }

    // --- Step Progression ---
    public void AdvanceStep()
    {
        if (stepSprites == null || stepSprites.Length == 0)
        {
            Debug.LogWarning("[PanStateHandler] No step sprites assigned.");
            return;
        }

        if (stepIndex < stepSprites.Length)
        {
            panImage.sprite = stepSprites[stepIndex];
            Debug.Log($"[PanStateHandler] Updated pan sprite to step {stepIndex + 1}");
            stepIndex++;
        }
        else
        {
            Debug.LogWarning("[PanStateHandler] No more step sprites available.");
        }
    }

    public void ResetPan()
    {
        panImage.sprite = defaultPanSprite;
        stepIndex = 0;
        Debug.Log("[PanStateHandler] Pan reset to default state.");
    }
}
