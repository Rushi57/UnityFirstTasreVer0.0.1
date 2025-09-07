using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            // Snap the ingredient to this drop zone
            eventData.pointerDrag.GetComponent<RectTransform>().position = transform.position;
        }
    }
}
