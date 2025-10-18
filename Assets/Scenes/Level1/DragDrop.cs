using UnityEngine;
using UnityEngine.EventSystems;

public class DragDropUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector3 startPosition;
    [SerializeField] private Canvas canvas; // Reference to your Canvas

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startPosition = rectTransform.position;
        canvasGroup.alpha = 0.6f;   // make transparent while dragging
        canvasGroup.blocksRaycasts = false; // allow drop detection
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Move ingredient following the mouse
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        // If not dropped on a valid drop zone → go back
        if (eventData.pointerEnter == null || eventData.pointerEnter.GetComponent<DropZoneUI>() == null)
        {
            rectTransform.position = startPosition;
        }
    }
}
