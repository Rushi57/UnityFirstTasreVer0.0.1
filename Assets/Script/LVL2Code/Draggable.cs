using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector2 originalPosition;
    private Transform originalParent;
    private Canvas canvas;
    private ItemData itemData;
    private bool droppedOnValidZone = false;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();
        originalParent = transform.parent;
        originalPosition = rectTransform.anchoredPosition;
        itemData = GetComponent<ItemData>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (itemData != null && !itemData.canDrag) return;

        droppedOnValidZone = false;

        originalParent = transform.parent;
        transform.SetParent(canvas.transform);
        transform.SetAsLastSibling();
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (itemData != null && !itemData.canDrag) return;
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (itemData != null && !itemData.canDrag) return;
        canvasGroup.blocksRaycasts = true;

        // If not dropped on any valid zone, return to start
        if (!droppedOnValidZone)
        {
            StartCoroutine(SmoothReturnToGrid());
        }
    }

    public void MarkAsDropped()
    {
        droppedOnValidZone = true;
    }

    public void RevertToOriginalPosition()
    {
        StartCoroutine(SmoothReturnToGrid());
    }

    private IEnumerator SmoothReturnToGrid()
    {
        // ✅ Step 1: Set parent back to original parent before animation
        transform.SetParent(originalParent);
        canvasGroup.blocksRaycasts = true;

        // ✅ Step 2: Wait one frame to let layout update (important)
        yield return null;

        // ✅ Step 3: Make sure anchored position is within the visible bounds
        RectTransform tableRect = originalParent.GetComponent<RectTransform>();
        RectTransform itemRect = rectTransform;

        Vector2 startPos = itemRect.anchoredPosition;
        Vector2 targetPos = originalPosition;

        // Clamp target position within parent bounds
        if (tableRect != null)
        {
            Vector2 halfSize = tableRect.rect.size / 2f;
            targetPos.x = Mathf.Clamp(targetPos.x, -halfSize.x, halfSize.x);
            targetPos.y = Mathf.Clamp(targetPos.y, -halfSize.y, halfSize.y);
        }

        // ✅ Step 4: Smooth move back
        float duration = 0.25f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            itemRect.anchoredPosition = Vector2.Lerp(startPos, targetPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        itemRect.anchoredPosition = targetPos;
    }

}
