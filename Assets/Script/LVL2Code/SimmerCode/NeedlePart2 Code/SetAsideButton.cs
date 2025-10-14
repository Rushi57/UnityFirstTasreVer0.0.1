using UnityEngine;

public class SetAsideButton : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private StoveDropZone stoveDropZone; // Reference to your StoveDropZone
    [SerializeField] private GameObject itemPrefab;       // Prefab that contains ItemData
    [SerializeField] private Transform tableZone3;        // Spawn point or parent
    [SerializeField] private ItemSO itemData;             // The ItemSO you want to assign

    public void SetSideButton()
    {
        // 1️⃣ Proceed to the next cooking step
        if (CookingStepManager.Instance != null)
        {
            Debug.Log("[SetAsideButton] Calling CookingStepManager.NextStep()");
            CookingStepManager.Instance.NextStep();
        }
        else
        {
            Debug.LogWarning("[SetAsideButton] CookingStepManager.Instance is NULL!");
        }

        // 2️⃣ Reset the stove
        if (stoveDropZone != null)
        {
            stoveDropZone.ResetStove();
            Debug.Log("[SetAsideButton] Stove reset successfully.");
        }
        else
        {
            Debug.LogWarning("[SetAsideButton] StoveDropZone reference is missing!");
        }

        // 3️⃣ Spawn the item prefab in TableZone3
        if (itemPrefab != null && tableZone3 != null)
        {
            GameObject newItem = Instantiate(itemPrefab, tableZone3.position, Quaternion.identity, tableZone3);
            Debug.Log($"[SetAsideButton] Spawned {newItem.name} at tableZone3.");

            // 4️⃣ Assign ItemSO to the prefab’s ItemData script
            ItemData itemDataComponent = newItem.GetComponent<ItemData>();
            if (itemDataComponent != null)
            {
                if (itemData != null)
                {
                    itemDataComponent.SetupItem(itemData);
                    Debug.Log($"[SetAsideButton] Assigned ItemSO '{itemData.name}' to {newItem.name}");
                }
                else
                {
                    Debug.LogWarning("[SetAsideButton] No ItemSO assigned in the Inspector!");
                }
            }
            else
            {
                Debug.LogWarning("[SetAsideButton] Spawned prefab has no ItemData component!");
            }
        }
        else
        {
            Debug.LogWarning("[SetAsideButton] Missing itemPrefab or tableZone3 reference!");
        }

        gameObject.SetActive(false);
        Debug.Log("Hide");
    }
}
