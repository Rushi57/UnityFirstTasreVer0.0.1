using UnityEngine;
using UnityEngine.UI;
using TMPro;

[ExecuteAlways] // lets you preview bar sizes in the Editor
public class MixingMechanicManager : MonoBehaviour
{
    [Header("UI References")]
    public RectTransform colorBar;
    public RectTransform indicator;
    public TextMeshProUGUI timerText;

    [Header("Zones")]
    public RectTransform zoneRed;
    public RectTransform zoneYellow;
    public RectTransform zoneGreen;

    [Header("Zone Heights (0–1 = percentage of total height)")]
    [Range(0f, 1f)] public float redHeightRatio = 0.6f;
    [Range(0f, 1f)] public float greenHeightRatio = 0.2f;
    [Range(0f, 1f)] public float yellowHeightRatio = 0.2f;

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
    private bool hasEnded = false;

    private float cachedBarHeight;

    private void Awake()
    {
        // Cache the color bar height once layout is updated
        Canvas.ForceUpdateCanvases();
        if (colorBar != null)
            cachedBarHeight = colorBar.rect.height;
    }

    private void Start()
    {
        ResetIndicator();
        timer = totalTime;
        SetupZones();

        if (closeButton != null)
            closeButton.onClick.AddListener(CloseAllPanels);

        if (completeShowPanel != null)
            completeShowPanel.SetActive(false);
    }

    private void Update()
    {
        if (!Application.isPlaying) return; // prevent updates in Edit Mode
        if (hasEnded) return;

        // Update timer
        if (timer > 0f)
        {
            timer -= Time.deltaTime;
            if (timer < 0f) timer = 0f;
        }

        if (timerText != null)
            timerText.text = Mathf.Ceil(timer).ToString() + "s";

        if (timer <= 0f)
        {
            EndMixing();
            return;
        }

        HandleIndicatorMovement();
    }

    private void HandleIndicatorMovement()
    {
        Vector2 pos = indicator.anchoredPosition;

        if (isRotating)
            pos.y += riseSpeed * Time.deltaTime;
        else
            pos.y -= fallSpeed * Time.deltaTime;

        float halfHeight = colorBar.rect.height / 2f;
        pos.y = Mathf.Clamp(pos.y, -halfHeight, halfHeight);
        indicator.anchoredPosition = pos;

        isRotating = false;
    }

    public void OnRotate() => isRotating = true;

    private void EndMixing()
    {
        if (hasEnded) return;
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
        float indicatorY = indicator.anchoredPosition.y;

        float greenMin = zoneGreen.anchoredPosition.y - zoneGreen.rect.height / 2f;
        float greenMax = zoneGreen.anchoredPosition.y + zoneGreen.rect.height / 2f;
        if (indicatorY >= greenMin && indicatorY <= greenMax)
            return ("Very Good", 6);

        float yellowMin = zoneYellow.anchoredPosition.y - zoneYellow.rect.height / 2f;
        float yellowMax = zoneYellow.anchoredPosition.y + zoneYellow.rect.height / 2f;
        if (indicatorY >= yellowMin && indicatorY <= yellowMax)
            return ("Good", 4);

        float redMin = zoneRed.anchoredPosition.y - zoneRed.rect.height / 2f;
        float redMax = zoneRed.anchoredPosition.y + zoneRed.rect.height / 2f;
        if (indicatorY >= redMin && indicatorY <= redMax)
            return ("Bad", 2);

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
        if (indicator != null && colorBar != null)
            indicator.anchoredPosition = new Vector2(indicator.anchoredPosition.x, -colorBar.rect.height / 2f);
    }

    private void SetupZones()
    {
        if (colorBar == null) return;

        float totalHeight = cachedBarHeight > 0 ? cachedBarHeight : colorBar.rect.height;

        // Normalize ratios if they exceed 1
        float totalRatio = redHeightRatio + greenHeightRatio + yellowHeightRatio;
        if (totalRatio > 1f)
        {
            redHeightRatio /= totalRatio;
            greenHeightRatio /= totalRatio;
            yellowHeightRatio /= totalRatio;
        }

        // Calculate zone heights
        float redHeight = totalHeight * redHeightRatio;
        float greenHeight = totalHeight * greenHeightRatio;
        float yellowHeight = totalHeight * yellowHeightRatio;

        float currentY = -totalHeight / 2f;

        // Red Zone (bottom)
        zoneRed.sizeDelta = new Vector2(zoneRed.sizeDelta.x, redHeight);
        zoneRed.anchoredPosition = new Vector2(0, currentY + redHeight / 2f);
        currentY += redHeight;

        // Green Zone (middle)
        zoneGreen.sizeDelta = new Vector2(zoneGreen.sizeDelta.x, greenHeight);
        zoneGreen.anchoredPosition = new Vector2(0, currentY + greenHeight / 2f);
        currentY += greenHeight;

        // Yellow Zone (top)
        zoneYellow.sizeDelta = new Vector2(zoneYellow.sizeDelta.x, yellowHeight);
        zoneYellow.anchoredPosition = new Vector2(0, currentY + yellowHeight / 2f);
    }


    private void CloseAllPanels()
    {
        if (mixingPanel != null)
            mixingPanel.SetActive(false);

        if (completeShowPanel != null)
            completeShowPanel.SetActive(false);
    }

    // 🧩 Optional: Auto-update zones in the Editor when you change ratios
    private void OnValidate()
    {
        if (!Application.isPlaying && colorBar != null)
        {
            Canvas.ForceUpdateCanvases();
            cachedBarHeight = colorBar.rect.height;
            SetupZones();
        }
    }
    public void RestartMixing()
    {
        Debug.Log("[MixingMechanicManager] Restarting Mixing Mechanic...");

        hasEnded = false;
        timer = totalTime;
        isRotating = false;

        // Reset UI states
        if (completeShowPanel != null)
            completeShowPanel.SetActive(false);

        if (mixingPanel != null)
            mixingPanel.SetActive(true);

        ResetIndicator();

        if (timerText != null)
            timerText.text = Mathf.Ceil(timer).ToString() + "s";

        Debug.Log("[MixingMechanicManager] Mixing reset complete.");
    }

}
