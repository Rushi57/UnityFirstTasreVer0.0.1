using UnityEngine;
using UnityEngine.EventSystems;

public class RotateUIObj : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    private RectTransform rectTransform;
    private Vector2 centerPoint;
    private float angleOffset;

    [Header("References")]
    public MixingMechanicManager mixingManager;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Store center of rotation
        centerPoint = RectTransformUtility.WorldToScreenPoint(eventData.pressEventCamera, rectTransform.position);

        Vector2 dir = eventData.position - centerPoint;
        angleOffset = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - rectTransform.eulerAngles.z;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 dir = eventData.position - centerPoint;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        rectTransform.rotation = Quaternion.Euler(0, 0, angle - angleOffset);

        // Tell manager that rotation happened this frame
        if (mixingManager != null)
            mixingManager.OnRotate();
    }
}
