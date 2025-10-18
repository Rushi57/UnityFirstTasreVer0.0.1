using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            RectTransform draggedObject = eventData.pointerDrag.GetComponent<RectTransform>();

            // Set this DropZone as the parent of the dragged object
            draggedObject.SetParent(transform);

            // Reset local position to center it
            draggedObject.localPosition = Vector3.zero;

            // (Optional) reset local scale and rotation to match the drop zone
            draggedObject.localRotation = Quaternion.identity;
            draggedObject.localScale = Vector3.one;
        }
    }
}