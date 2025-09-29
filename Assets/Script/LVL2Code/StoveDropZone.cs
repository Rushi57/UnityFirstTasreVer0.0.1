using UnityEngine;
using UnityEngine.EventSystems;

public class StoveDropZone : MonoBehaviour, IDropHandler
{
    [Header("References")]
    public GameObject panPrefab;   // Prefab with PanStateHandler
    public GameObject mixingPanel; // Scene UI  mixing mini-game
    public GameObject simmerClockPanel; // Scene UI simmer mini-game

    private GameObject currentPan; // Spawned pan instance

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
        {
            Debug.LogWarning("[DropZone] pointerDrag is null");
            return;
        }


        ItemData draggedData = eventData.pointerDrag.GetComponent<ItemData>();
        if (draggedData == null || draggedData.itemSO == null)
        {
            Debug.LogWarning("[DropZone] No ItemData on dragged object");
            return;
        }

        ItemSO item = draggedData.itemSO;
        Debug.Log($"[DropZone] Dropped item: {item.itemName}, Type: {item.itemType}");

        // === 1️⃣ Place the Pan ===
        if (item.itemType == ItemType.Utility && item.itemName == "Pan")
        {
            if (currentPan == null)
            {
                // ⬇️ The updated block starts here
                currentPan = Instantiate(panPrefab, transform);
                currentPan.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

                // Assign the Mixing Panel reference to the spawned PanStateHandler
                PanStateHandler handler = currentPan.GetComponent<PanStateHandler>();
                if (handler != null)
                {
                    handler.mixingMechPanel = mixingPanel;
                }

                //Assign the Simmer Panel reference to the spawned PanstateHandler 
                PanStateHandler simmerhandler = currentPan.GetComponent<PanStateHandler>();
                if (handler != null)
                {
                    simmerhandler.simmerClockPanel = simmerClockPanel;
                }


                // ⬆️ Updated block ends here

                Debug.Log("✅ Pan placed on stove");
            }

            Destroy(eventData.pointerDrag);
            CookingStepManager.Instance.NextStep();
            return;
        }

        // === 2️⃣ Ingredient Handling ===
        if (currentPan != null && item.itemType == ItemType.Ingredient)
        {
            bool correct = CookingStepManager.Instance.IsCorrectItem(item);
            if (correct)
            {
                PanStateHandler panHandler = currentPan.GetComponent<PanStateHandler>();
                if (panHandler != null)
                {
                    panHandler.UpdatePanIngredient();
                }

                CookingStepManager.Instance.NextStep();
                Destroy(eventData.pointerDrag);
            }
            else
            {
                draggedData.GetComponent<Draggable>()?.RevertToOriginalPosition();
                CookingStepManager.Instance.WrongAttempt();
            }
            return;
        }

        

        // === Nothing matched ===
        Debug.Log("[DropZone] Drop ignored (no matching condition).");
    }
}
