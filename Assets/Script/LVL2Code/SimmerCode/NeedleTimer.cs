using UnityEngine;
using UnityEngine.UI;

public class NeedleTimer : MonoBehaviour
{
    public RectTransform needle;       // drag your needle here
    public float speed = 180f;         // degrees per second
    public bool rotating = true;

    [Header("Sectors (degrees)")]
    [Tooltip("startAngle is clockwise from 0° (pointing up)")]
    public float yellowStart = 330f;   // example values
    public float yellowEnd = 20f;    // wrap-around allowed
    public float greenStart = 20f;
    public float greenEnd = 60f;

    public Text resultText;

    void Update()
    {
        if (rotating)
            needle.Rotate(0f, 0f, -speed * Time.deltaTime);
    }

    public void StopNeedle()
    {
        rotating = false;

        // Z rotation is negative when rotating clockwise
        float angle = needle.eulerAngles.z;
        // Convert so 0° is up and increases clockwise
        angle = (360f - angle + 90f) % 360f;

        string result;
        if (IsAngleInSector(angle, greenStart, greenEnd))
            result = "Very Good";
        else if (IsAngleInSector(angle, yellowStart, yellowEnd))
            result = "Good";
        else
            result = "Bad";

        Debug.Log($"Angle {angle:F1}° → {result}");
        if (resultText) resultText.text = result;
    }

    bool IsAngleInSector(float angle, float start, float end)
    {
        // Handles wrap-around (e.g. start 330 end 20)
        if (start < end)
            return angle >= start && angle <= end;
        else
            return angle >= start || angle <= end;
    }
}
