using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PanStateHandler : MonoBehaviour, IDropHandler
{
    [Header("UI Reference")]
    public GameObject mixingMechPanel;   //MixingPanel
    public GameObject simmerClockPanel;

    [Header("Pan Images")]
    public Image panImage;
    public Sprite defaultPanSprite;

    [Header("Ingredient step sprites")]
    public Sprite[] stepSprites;     // ordered per ingredient step

    [Header("Action sprites")]
    public Sprite oilPanSprite;
    public Sprite vinegarPanSprite;
    public Sprite soyPanSprite;

    private int stepIndex = 0;

    public void OnDrop(PointerEventData eventData)
    {
        var droppedItem = eventData.pointerDrag?.GetComponent<ItemData>();
        if (droppedItem == null) return;

        // --- Ingredient drop ---
        if (droppedItem.itemSO.itemType == ItemType.Ingredient)
        {
            if (CookingStepManager.Instance.IsCorrectItem(droppedItem.itemSO))
            {
                UpdatePanIngredient();
                CookingStepManager.Instance.NextStep();

                Debug.Log("Destroying accepted ingredient: " + eventData.pointerDrag.name);
                Destroy(eventData.pointerDrag.gameObject); // ✅ remove from scene
            }
            else
            {
                droppedItem.GetComponent<Draggable>()?.RevertToOriginalPosition();
                CookingStepManager.Instance.WrongAttempt();
            }
        }
        // --- Mix/Spatula step ---
        if (CookingStepManager.Instance.IsCorrectAction("Mix")
            && droppedItem.itemSO.itemName.Equals("Spatula", System.StringComparison.OrdinalIgnoreCase))
        {
            // ✅ Correct action
            CookingStepManager.Instance.NextStep();

            // open the mixing panel
            mixingMechPanel.SetActive(true);

            Destroy(eventData.pointerDrag.gameObject);
            return;
        }
        // --- Simmer/PanLid step ---
        if (CookingStepManager.Instance.IsCorrectAction("Simmer")
            && droppedItem.itemSO.itemName.Equals("PanLid", System.StringComparison.OrdinalIgnoreCase))
        {
            // ✅ Correct action
            CookingStepManager.Instance.NextStep();

            // open the simmer panel
            simmerClockPanel.SetActive(true);

            Destroy(eventData.pointerDrag.gameObject);
            return;
        }

        // --- Action drop (condiments) ---
        else if (droppedItem.itemSO.itemType == ItemType.Action)
        {
            string actionName = droppedItem.itemSO.itemName;
            if (CookingStepManager.Instance.OnActionPerformed(actionName))
            {
                UpdatePanWithAction(actionName);
                Debug.Log("Destroying accepted action item: " + eventData.pointerDrag.name);
                Destroy(eventData.pointerDrag.gameObject); // ✅ remove from scene
            }
            else
            {
                droppedItem.GetComponent<Draggable>()?.RevertToOriginalPosition();
            }
        }
    }

    public void UpdatePanIngredient()
    {
        if (stepSprites == null || stepSprites.Length == 0) return;

        if (stepIndex < stepSprites.Length)
        {
            panImage.sprite = stepSprites[stepIndex];
            stepIndex++;
        }
        else
        {
            Debug.LogWarning("No more ingredient sprites available.");
        }
    }

    public void UpdatePanWithAction(string action)
    {
        if (string.IsNullOrEmpty(action)) return;

        switch (action.Trim().ToLower())
        {
            case "oil":
                if (oilPanSprite) panImage.sprite = oilPanSprite;
                break;
            case "vinegar":
                if (vinegarPanSprite) panImage.sprite = vinegarPanSprite;
                break;
            case "soy":
            case "soy sauce":
                if (soyPanSprite) panImage.sprite = soyPanSprite;
                break;
        }
    }

    public void ResetPan()
    {
        panImage.sprite = defaultPanSprite;
        stepIndex = 0;
    }
}
