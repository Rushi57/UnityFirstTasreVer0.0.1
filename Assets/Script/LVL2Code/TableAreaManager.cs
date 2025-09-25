using UnityEngine;

public class TableAreaManager : MonoBehaviour
{
    [SerializeField] private Transform tableArea;
    [SerializeField] private float spacing = 150f; // distance between slots

    public void AddToTable(GameObject slotPrefab, ItemSO itemSO)
    {
        int childCount = tableArea.childCount;

        // spawn prefab
        GameObject newSlot = Instantiate(slotPrefab, tableArea);

        // position with spacing
        RectTransform rt = newSlot.GetComponent<RectTransform>();
        rt.anchoredPosition = new Vector2(childCount * spacing, 0);

        // setup data
        ItemData data = newSlot.GetComponent<ItemData>();
        if (data != null)
        {
            data.SetupItem(itemSO);
        }
    }
}
