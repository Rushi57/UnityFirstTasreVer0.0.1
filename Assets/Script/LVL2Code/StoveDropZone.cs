using UnityEngine;
using UnityEngine.EventSystems;

public class StoveDropZone : MonoBehaviour, IDropHandler
{
    public GameObject panPrefab;   // Prefab with PanStateHandler
    private GameObject currentPan; // Reference to the spawned pan
    public GameObject mixingPanel; // Reference to MixingMechPanel in Canvas

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

            Destroy(eventData.pointerDrag);
            CookingStepManager.Instance.NextStep();
            return;
        }

        // --- Check if Spatula ---
        if (droppedItem.itemSO.itemType == ItemType.Utility && droppedItem.itemSO.itemName == "Spatula")
        {
            string expected = CookingStepManager.Instance.GetExpectedStep();
            if (expected == "Utility:Spatula")
            {
                Debug.Log("🥄 Spatula dropped! Opening Mixing Panel...");
                mixingPanel.SetActive(true);

                CookingStepManager.Instance.NextStep();
                Destroy(eventData.pointerDrag); // Remove from table
            }
            else
            {
                Debug.Log("❌ Spatula not expected yet");
                droppedItem.GetComponent<Draggable>()?.RevertToOriginalPosition();
                CookingStepManager.Instance.WrongAttempt();
            }
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
                    panHandler.UpdatePan(droppedItem.itemSO);
                }

                CookingStepManager.Instance.NextStep();
                Destroy(eventData.pointerDrag);
            }
            else
            {
                droppedItem.GetComponent<Draggable>()?.RevertToOriginalPosition();
                CookingStepManager.Instance.WrongAttempt();
            }
        }
    }
}
