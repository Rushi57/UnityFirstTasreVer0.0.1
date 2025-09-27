using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MixingMechanicManager : MonoBehaviour
{
    [Header("UI References")]
    public RectTransform colorBar;     // Parent container
    public RectTransform indicator;    // The moving indicator
    public TextMeshProUGUI timerText;

    [Header("Zones (fixed order)")]
    public RectTransform zoneRed;      // Bottom
    public RectTransform zoneYellow;   // Middle
    public RectTransform zoneGreen;    // Top

    [Header("Gameplay Settings")]
    public float riseSpeed = 50f;
    public float fallSpeed = 20f;
    public float totalTime = 10f;

    private float timer;
    private bool isRotating = false;

    private void Start()
    {
        ResetIndicator();
        timer = totalTime;
        SetupFixedZones();
    }

    private void Update()
    {
        // Timer update
        if (timer > 0f)
        {
            timer -= Time.deltaTime;
            if (timer < 0f) timer = 0f;
        }

        timerText.text = Mathf.Ceil(timer).ToString() + "s";

        if (timer <= 0f)
        {
            EndMixing();
            return;
        }

        // Indicator rising/falling
        Vector2 pos = indicator.anchoredPosition;
        if (isRotating) pos.y += riseSpeed * Time.deltaTime;
        else pos.y -= fallSpeed * Time.deltaTime;

        float halfHeight = colorBar.rect.height / 2f;
        pos.y = Mathf.Clamp(pos.y, -halfHeight, halfHeight);
        indicator.anchoredPosition = pos;

        isRotating = false;
    }

    public void OnRotate() => isRotating = true;

    private void EndMixing()
    {
        string score = GetCurrentZone();
        Debug.Log("Final Score: " + score);
        enabled = false;
    }

    private string GetCurrentZone()
    {
        if (RectTransformUtility.RectangleContainsScreenPoint(zoneRed, indicator.position))
            return "Bad = 50%";
        if (RectTransformUtility.RectangleContainsScreenPoint(zoneYellow, indicator.position))
            return "Good = 75%";
        if (RectTransformUtility.RectangleContainsScreenPoint(zoneGreen, indicator.position))
            return "Very Good = 100%";

        return "No Zone";
    }

    public void ResetIndicator()
    {
        indicator.anchoredPosition = new Vector2(indicator.anchoredPosition.x, -colorBar.rect.height / 2f);
    }

    private void SetupFixedZones()
    {
        float totalHeight = colorBar.rect.height;
        float redHeight = totalHeight * 0.59f;
        float yellowHeight = totalHeight * 0.16f;
        float greenHeight = totalHeight * 0.25f;

        // Bottom (Red)
        zoneRed.sizeDelta = new Vector2(zoneRed.sizeDelta.x, redHeight);
        zoneRed.anchoredPosition = new Vector2(0, -totalHeight / 2f + redHeight / 2f);

        // Middle (Yellow)
        zoneYellow.sizeDelta = new Vector2(zoneYellow.sizeDelta.x, yellowHeight);
        zoneYellow.anchoredPosition = new Vector2(0, zoneRed.anchoredPosition.y + redHeight / 2f + yellowHeight / 2f);

        // Top (Green)
        zoneGreen.sizeDelta = new Vector2(zoneGreen.sizeDelta.x, greenHeight);
        zoneGreen.anchoredPosition = new Vector2(0, zoneYellow.anchoredPosition.y + yellowHeight / 2f + greenHeight / 2f);
    }
}
