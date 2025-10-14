using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PanStateHandler : MonoBehaviour, IDropHandler
{
    [Header("UI Reference")]
    public GameObject mixingMechPanel;   // MixingPanel
    public GameObject simmerClockPanel;
    public GameObject boilClockPanel;
    [Header("Pan Images")]
    public Image panImage;
    public Sprite defaultPanSprite;

    [Header("Ingredient Step Sprites")]
    public Sprite[] stepSprites; // ordered per ingredient or action step

    [Header("UI Checkmarks")]
    public GameObject uncheckedImage1; // Assign in Inspector
    public GameObject uncheckedImage2; // Assign in Inspector

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
        else if (droppedItem.itemSO.itemName.Equals("Spatula", System.StringComparison.OrdinalIgnoreCase))
        {
            if (CookingStepManager.Instance.IsCorrectAction("Mix"))
            {
                if (mixingMechPanel != null)
                {
                    mixingMechPanel.SetActive(true);

                    var mixManager = mixingMechPanel.GetComponent<MixingMechanicManager>();
                    if (mixManager != null)
                    {
                        mixManager.RestartMixing();
                    }
                }

                CookingStepManager.Instance.NextStep();
                Debug.Log("[PanStateHandler] Mix action triggered.");
                Destroy(eventData.pointerDrag.gameObject);
            }
            else
            {
                droppedItem.GetComponent<Draggable>()?.RevertToOriginalPosition();
                CookingStepManager.Instance.WrongAttempt();
            }
        }

        // --- SIMMER / PAN LID STEP ---
        else if (droppedItem.itemSO.itemName.Contains("Cover") || droppedItem.itemSO.itemName.Contains("Lid"))
        {
            Debug.Log($"[PanStateHandler] Lid/Cover detected for {droppedItem.itemSO.itemName}");

            // 🔹 Handle Simmer
            if (CookingStepManager.Instance.IsCorrectAction("Simmer"))
            {
                Debug.Log("[PanStateHandler] Simmer action triggered (Lid)");

                if (simmerClockPanel != null)
                {
                    simmerClockPanel.SetActive(true);
                    simmerClockPanel.GetComponent<NeedleTimer>()?.RestartSimmer();
                }

                CookingStepManager.Instance.TrySimmer();
                CookingStepManager.Instance.NextStep();
            }
            // Handle Boil
            else if (CookingStepManager.Instance.IsCorrectAction("Boil"))
            {
                Debug.Log("[PanStateHandler] Boil action triggered (Cover)");

                if (boilClockPanel != null)
                {
                    boilClockPanel.SetActive(true);
                    boilClockPanel.GetComponent<BoilTimer>()?.RestartBoil();
                }

                CookingStepManager.Instance.TryBoil();
            }
            else
            {
                Debug.LogWarning("[PanStateHandler] Cover/Lid dropped but not the expected step.");
                CookingStepManager.Instance.WrongAttempt();
            }

            // Always destroy the dragged item
            Destroy(eventData.pointerDrag.gameObject);
        }





        // --- CONDIMENT / ACTION DROP ---
        else if (droppedItem.itemSO.itemType == ItemType.Action)
        {
            string actionName = droppedItem.itemSO.itemName.Trim();
            Debug.Log($"[PanStateHandler] Detected Action Drop: {actionName}");

            if (CookingStepManager.Instance.OnActionPerformed(actionName))
            {
                AdvanceStep();
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

            // ✅ Show unchecked images only when Element 4 (index 4) is done
            if (stepIndex == 4)
            {
                if (uncheckedImage1 != null) uncheckedImage1.SetActive(true);
                if (uncheckedImage2 != null) uncheckedImage2.SetActive(true);
                Debug.Log("[PanStateHandler] Unchecked images shown (step 4 done).");
            }
            // ✅ Hide both automatically on next step (Element 5)
            else if (stepIndex == 5)
            {
                if (uncheckedImage1 != null) uncheckedImage1.SetActive(false);
                if (uncheckedImage2 != null) uncheckedImage2.SetActive(false);
                Debug.Log("[PanStateHandler] Unchecked images hidden (step 5 started).");
            }

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

        if (uncheckedImage1 != null) uncheckedImage1.SetActive(false);
        if (uncheckedImage2 != null) uncheckedImage2.SetActive(false);

        Debug.Log("[PanStateHandler] Pan reset to default state.");
    }
}
