using UnityEngine;
using UnityEngine.EventSystems;

public class StoveDropZone : MonoBehaviour, IDropHandler
{
    public Vector3 itemScale = new Vector3(2f, 2f, 2f); // bigger size in stove
    private CookbookManager cookbook;

    private void Start()
    {
        cookbook = FindObjectOfType<CookbookManager>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        DragItems item = eventData.pointerDrag.GetComponent<DragItems>();
        if (item != null)
        {
            item.transform.SetParent(transform);
            item.transform.position = transform.position; // snap center
            item.transform.localScale = itemScale;

            Debug.Log(item.itemName + " placed in Stove.");

            // ✅ Check recipe step
            if (cookbook != null)
            {
                cookbook.TryAddIngredient(item.itemName);
            }
        }
    }
}
