using UnityEngine;
using UnityEngine.EventSystems;

public class RotateUIObj : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    private RectTransform rectTransform;
    private Vector2 centerPoint;
    private float angleOffset;
    private float targetAngle;
    private bool isTouching = false;

    [Header("Rotation Settings")]
    [Tooltip("Higher = faster rotation response")]
    public float smoothSpeed = 15f;

    [Header("References")]
    public MixingMechanicManager mixingManager;

    [Header("Audio Setting")]
    [Tooltip("Sound effect play")]
    public AudioClip rotationSfx;
    private AudioSource audioSource;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();

        //Prepare the Audio Source
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.playOnAwake = false;
        audioSource.volume = SFXManager.Instance != null ? SFXManager.Instance.GetVolume() : 1f;

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (Input.touchCount > 0 || Input.GetMouseButtonDown(0))
        {
            isTouching = true;
            SetCenter(eventData);

            //Audio rotation SFX
            if(rotationSfx != null)
            {
                audioSource.clip = rotationSfx;
                audioSource.volume = SFXManager.Instance != null ? SFXManager.Instance.GetVolume() : 1f;
                audioSource.Play();
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isTouching) return;

        Vector2 dir = eventData.position - centerPoint;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        targetAngle = angle - angleOffset;

        // ✅ Notify mixing manager each drag
        if (mixingManager != null)
            mixingManager.OnRotate();
    }

    private void Update()
    {
        // ✅ Smoothly rotate toward the latest targetAngle every frame
        if (isTouching)
        {
            Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);
            rectTransform.rotation = Quaternion.Lerp(
                rectTransform.rotation,
                targetRotation,
                Time.deltaTime * smoothSpeed
            );
        }

        // ✅ Reset when touch ends
        if (isTouching && Input.touchCount == 0 && !Input.GetMouseButton(0))
        {
            isTouching = false;
            if(audioSource.isPlaying)
                audioSource.Stop();
        }
    }

    private void SetCenter(PointerEventData eventData)
    {
        centerPoint = RectTransformUtility.WorldToScreenPoint(eventData.pressEventCamera, rectTransform.position);

        Vector2 dir = eventData.position - centerPoint;
        angleOffset = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - rectTransform.eulerAngles.z;
        targetAngle = rectTransform.eulerAngles.z; // initialize target
    }
}
