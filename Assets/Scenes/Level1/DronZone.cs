using UnityEngine;
using UnityEngine.EventSystems;

public class DropZoneUI : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            // Snap ingredient to this UI Image position
            RectTransform draggedObj = eventData.pointerDrag.GetComponent<RectTransform>();
            draggedObj.position = transform.position;
        }
    }
}
