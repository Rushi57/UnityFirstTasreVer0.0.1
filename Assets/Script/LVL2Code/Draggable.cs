using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector2 originalPosition;
    private Transform originalParent;
    private Canvas canvas;
    private ItemData itemData;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();
        originalPosition = rectTransform.anchoredPosition;
        originalParent = transform.parent;
        itemData = GetComponentInParent<ItemData>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(!itemData.canDrag) return;

        transform.SetParent(originalParent.root);
        transform.SetAsLastSibling();
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(!itemData.canDrag) return;
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(!itemData.canDrag) return;
        canvasGroup.blocksRaycasts = true;
    }

    public void RevertToOriginalPosition()
    {
        transform.position = originalPosition;
    }

}
