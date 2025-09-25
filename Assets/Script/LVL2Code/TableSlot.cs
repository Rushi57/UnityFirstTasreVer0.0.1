using UnityEngine;

public class TableSlot : MonoBehaviour
{
    [HideInInspector] public GameObject currentItem;

    public bool IsEmpty => transform.childCount == 0;

    public void PlaceItem(GameObject itemPrefab, ItemSO itemSO)
    {
        if (!IsEmpty)
        {
            Debug.LogWarning($"Slot {name} already has an item.");
            return;
        }

        GameObject go = Instantiate(itemPrefab, transform);
        RectTransform rt = go.GetComponent<RectTransform>();
        if (rt != null)
        {
            rt.anchoredPosition = Vector2.zero;
            rt.localScale = Vector3.one;
        }

        ItemData data = go.GetComponent<ItemData>();
        if (data != null)
        {
            data.SetupItem(itemSO);
            data.originalParent = transform;
            if (rt != null) data.originalAnchoredPosition = rt.anchoredPosition;
        }

        currentItem = go;
    }

    public void ClearSlot()
    {
        if (currentItem != null) Destroy(currentItem);
        currentItem = null;
    }
}
