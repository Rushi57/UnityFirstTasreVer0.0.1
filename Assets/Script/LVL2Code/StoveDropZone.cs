using UnityEngine;
using UnityEngine.EventSystems;

public class StoveDropZone : MonoBehaviour, IDropHandler
{
    public GameObject panPrefab;   // Prefab with Image + PanStateHandler
    private GameObject currentPan; // Reference to the spawned pan

    public void OnDrop(PointerEventData eventData)
    {
        ItemData droppedItem = eventData.pointerDrag.GetComponent<ItemData>();
        if (droppedItem == null || droppedItem.itemSO == null) return;

        // --- Check if Pan ---
        if (droppedItem.itemSO.itemType == ItemType.Utility && droppedItem.itemSO.itemName == "Pan")
        {
            if (currentPan == null) // Spawn only once
            {
                currentPan = Instantiate(panPrefab, transform);
                currentPan.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

                Debug.Log("✅ Pan placed on stove");
            }

            Destroy(eventData.pointerDrag); // Remove draggable pan from TableArea
            CookingStepManager.Instance.NextStep();
            return;
        }

        // --- Check if Ingredient ---
        if (currentPan != null && droppedItem.itemSO.itemType == ItemType.Ingredient)
        {
            if (CookingStepManager.Instance.IsCorrectItem(droppedItem.itemSO))
            {
                var panHandler = currentPan.GetComponent<PanStateHandler>();
                if (panHandler != null)
                {
                    panHandler.UpdatePan(droppedItem.itemSO); // Change sprite/state
                }

                CookingStepManager.Instance.NextStep();
                Destroy(eventData.pointerDrag); // Ingredient disappears into pan
            }
            else
            {
                // Return ingredient to original slot
                Draggable drag = droppedItem.GetComponent<Draggable>();
                if (drag != null)
                    drag.RevertToOriginalPosition();

                CookingStepManager.Instance.WrongAttempt();
            }
        }
    }
}
