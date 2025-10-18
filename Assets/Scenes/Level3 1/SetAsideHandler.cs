using UnityEngine;

public class SetAsideHandler : MonoBehaviour
{
    [Header("References")]
    public GameObject ingredientPrefab;   // e.g. Boiled Pork Prefab
    public Transform stoveDropZone;       // StoveDropZone transform
    public StoveDropZone stoveDropZoneScript; // Reference to reset stove

    public void HandleSetAside()
    {
        Debug.Log("[SetAsideHandler] Set Aside button clicked.");

        if (ingredientPrefab != null && stoveDropZone != null)
        {
            Instantiate(ingredientPrefab, stoveDropZone);
            Debug.Log("[SetAsideHandler] Spawned ingredient prefab!");
        }
        else
        {
            Debug.LogWarning("[SetAsideHandler] Missing ingredientPrefab or stoveDropZone reference!");
        }

        // ✅ Reset stove if reference exists
        if (stoveDropZoneScript != null)
        {
            stoveDropZoneScript.ResetStove();
            Debug.Log("[SetAsideHandler] Stove reset complete.");
        }

        // ✅ Move to next step in the recipe
        if (CookingStepManager.Instance != null)
            CookingStepManager.Instance.NextStep();
    }
}
