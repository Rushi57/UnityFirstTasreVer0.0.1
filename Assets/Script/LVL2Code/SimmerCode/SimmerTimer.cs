using UnityEngine;
using UnityEngine.UI;

public class SimmerTimer : MonoBehaviour
{
    [Header("UI References")]
    public Image yellowZone;
    public Image greenZone;
    public RectTransform needle;

    [Header("Zone Sizes (degrees)")]
    [Range(0, 360)] public float yellowSize = 90f;
    [Range(0, 360)] public float greenSize = 45f;

    [Header("Start Angle")]
    [Range(0, 360)] public float startAngle = 0f;

    [Header("Needle / Gameplay")]
    public float rotationSpeed = 180f;
    public bool startRunning = true;

    private Vector2 yellowRange;
    private Vector2 greenRange;
    private bool isRunning;

    void Start()
    {
        SetupZones();
        isRunning = startRunning;
    }

    void Update()
    {
        if (!isRunning) return;

        needle.Rotate(0, 0, -rotationSpeed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space)) OnClickStop();
    }

    private void SetupZones()
    {
        // clamp total
        float total = yellowSize + greenSize;
        if (total > 360f) total = 360f;

        // yellow
        float yellowStart = NormalizeAngle(startAngle);
        float yellowEnd = NormalizeAngle(yellowStart + yellowSize);
        yellowRange = new Vector2(yellowStart, yellowEnd);
        SetupSliceImage(yellowZone, yellowSize, yellowStart);

        // green
        float greenStart = yellowEnd;
        float greenEnd = NormalizeAngle(greenStart + greenSize);
        greenRange = new Vector2(greenStart, greenEnd);
        SetupSliceImage(greenZone, greenSize, greenStart);
    }

    private void SetupSliceImage(Image img, float sizeDeg, float startDeg)
    {
        if (img == null) return;

        img.type = Image.Type.Filled;
        img.fillMethod = Image.FillMethod.Radial360;
        img.fillOrigin = (int)Image.Origin360.Top;
        img.fillClockwise = true;

        img.fillAmount = sizeDeg / 360f;
        img.rectTransform.localEulerAngles = new Vector3(0, 0, -startDeg);
    }

    public void OnClickStop()
    {
        isRunning = false;
        float z = NormalizeAngle(needle.eulerAngles.z);

        if (IsInRange(z, greenRange))
            Debug.Log("Very Good (Green)");
        else if (IsInRange(z, yellowRange))
            Debug.Log("Good (Yellow)");
        else
            Debug.Log("Bad (Red)");
    }

    private bool IsInRange(float angle, Vector2 range)
    {
        if (range.x < range.y)
            return angle >= range.x && angle < range.y;
        else
            return angle >= range.x || angle < range.y;
    }

    private float NormalizeAngle(float a) => (a % 360 + 360) % 360;
}
