using UnityEngine;
using UnityEngine.EventSystems;

public class TableZone : MonoBehaviour,IDropHandler
{
    public Vector3 itemScale = new Vector3(1f, 1f, 1f);

    public void OnDrop(PointerEventData eventData)
    {
        DragItems item = eventData.pointerDrag.GetComponent<DragItems>();
        if (item != null )
        {
            item.transform.SetParent(transform);
            item.transform.position = transform.position; // snap center
            item.transform.localScale = itemScale;
        }
    }
}
