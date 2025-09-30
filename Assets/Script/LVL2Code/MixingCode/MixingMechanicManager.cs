using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MixingMechanicManager : MonoBehaviour
{
    [Header("UI References")]
    public RectTransform colorBar;
    public RectTransform indicator;
    public TextMeshProUGUI timerText;

    [Header("Zones (fixed order)")]
    public RectTransform zoneRed;
    public RectTransform zoneYellow;
    public RectTransform zoneGreen;

    [Header("Gameplay Settings")]
    public float riseSpeed = 50f;
    public float fallSpeed = 20f;
    public float totalTime = 10f;

    [Header("Panels")]
    public GameObject mixingPanel;
    public GameObject completeShowPanel;
    public TextMeshProUGUI resultText;
    public Button closeButton;

    private float timer;
    private bool isRotating = false;
    private bool hasEnded = false;   // ✅ prevents scoring multiple times

    private void Start()
    {
        ResetIndicator();
        timer = totalTime;
        SetupFixedZones();

        if (closeButton != null)
            closeButton.onClick.AddListener(CloseAllPanels);

        if (completeShowPanel != null)
            completeShowPanel.SetActive(false);
    }

    private void Update()
    {
        if (hasEnded) return;   // ✅ stop everything after mixing ends

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

        // Indicator movement
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
        if (hasEnded) return; // ✅ safeguard
        hasEnded = true;

        var result = GetCurrentZone();

        if (result.HasValue)
        {
            Debug.Log($"Final Score: {result.Value.label} ({result.Value.score} pts)");

            if (TotalScoreManager.Instance != null)
                TotalScoreManager.Instance.AddMixScore(result.Value.score);

            if (completeShowPanel != null)
            {
                completeShowPanel.SetActive(true);
                if (resultText != null)
                    resultText.text = result.Value.label;
            }
        }
        else
        {
            Debug.Log("No zone hit — no score awarded.");
            if (completeShowPanel != null)
            {
                completeShowPanel.SetActive(true);
                if (resultText != null)
                    resultText.text = "No Score";
            }
        }
    }

    private (string label, int score)? GetCurrentZone()
    {
        Vector2 localPoint = indicator.localPosition;

        if (IsInside(localPoint, zoneGreen)) return ("Very Good", 6);
        if (IsInside(localPoint, zoneYellow)) return ("Good", 4);
        if (IsInside(localPoint, zoneRed)) return ("Bad", 2);

        return null;
    }

    private bool IsInside(Vector2 point, RectTransform zone)
    {
        Vector2 zoneCenter = zone.localPosition;
        Vector2 halfSize = zone.rect.size / 2f;

        return (point.x >= zoneCenter.x - halfSize.x &&
                point.x <= zoneCenter.x + halfSize.x &&
                point.y >= zoneCenter.y - halfSize.y &&
                point.y <= zoneCenter.y + halfSize.y);
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

        // Red (Bottom)
        zoneRed.sizeDelta = new Vector2(zoneRed.sizeDelta.x, redHeight);
        zoneRed.anchoredPosition = new Vector2(0, -totalHeight / 2f + redHeight / 2f);

        // Yellow (Middle)
        zoneYellow.sizeDelta = new Vector2(zoneYellow.sizeDelta.x, yellowHeight);
        zoneYellow.anchoredPosition = new Vector2(0, zoneRed.anchoredPosition.y + redHeight / 2f + yellowHeight / 2f);

        // Green (Top)
        zoneGreen.sizeDelta = new Vector2(zoneGreen.sizeDelta.x, greenHeight);
        zoneGreen.anchoredPosition = new Vector2(0, zoneYellow.anchoredPosition.y + yellowHeight / 2f + greenHeight / 2f);
    }

    private void CloseAllPanels()
    {
        if (mixingPanel != null)
            mixingPanel.SetActive(false);

        if (completeShowPanel != null)
            completeShowPanel.SetActive(false);
       
    }
}
