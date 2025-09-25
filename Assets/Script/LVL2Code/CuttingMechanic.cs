using UnityEngine;
using UnityEngine.UI;

public class CuttingMechanic : MonoBehaviour
{
    [Header("UI References")]
    public GameObject cuttingPanel;
    public Image ingredientDisplayImg;
    public Button tapToCutBtn;
    public RectTransform arrowIndicator;
    public RectTransform colorBar; // the single white bar

    [Header("Knife Settings")]
    public float knifeSpeed = 600f;

    private bool goingRight = true;
    private ItemData currentItem;

    [Header("Debug Overlays (optional)")]
    public Image yellowOverlay;
    public Image greenOverlay;

    // Percent ranges (0–1)
    private float yellowStart, yellowEnd;
    private float greenStart, greenEnd;

    void Start()
    {
        if (cuttingPanel != null)
            cuttingPanel.SetActive(false);

        if (tapToCutBtn != null)
            tapToCutBtn.onClick.AddListener(EvaluateCut);
    }

    void Update()
    {
        if (cuttingPanel == null || !cuttingPanel.activeSelf) return;

        float halfWidth = colorBar.rect.width / 2;

        float move = knifeSpeed * Time.deltaTime * (goingRight ? 1 : -1);
        arrowIndicator.anchoredPosition += new Vector2(move, 0);

        if (arrowIndicator.anchoredPosition.x >= halfWidth)
            goingRight = false;
        else if (arrowIndicator.anchoredPosition.x <= -halfWidth)
            goingRight = true;
    }

    public void StartCutting(Sprite ingredientSprite, ItemData item)
    {
        currentItem = item;
        cuttingPanel.SetActive(true);
        ingredientDisplayImg.sprite = ingredientSprite;

        // Reset knife
        arrowIndicator.anchoredPosition = new Vector2(-colorBar.rect.width / 2, 0);

        // Randomize zones
        SetupZones();
    }

    private void SetupZones()
    {
        float totalWidth = colorBar.rect.width;

        // Yellow = 70%
        float yellowWidth = 0.5f;
        yellowStart = Random.Range(0f, 1f - yellowWidth);
        yellowEnd = yellowStart + yellowWidth;

        // Green = 30% inside yellow
        float greenWidth = 0.1f;
        greenStart = Random.Range(yellowStart, yellowEnd - greenWidth);
        greenEnd = greenStart + greenWidth;

        Debug.Log($"Zones → Yellow: {yellowStart:P0}-{yellowEnd:P0}, Green: {greenStart:P0}-{greenEnd:P0}");

        // --- Debug overlays ---
        if (yellowOverlay != null)
        {
            float yStartPos = (yellowStart * totalWidth) - totalWidth / 2f;
            float yWidth = (yellowEnd - yellowStart) * totalWidth;
            yellowOverlay.rectTransform.sizeDelta = new Vector2(yWidth, yellowOverlay.rectTransform.sizeDelta.y);
            yellowOverlay.rectTransform.anchoredPosition = new Vector2(yStartPos + yWidth / 2, 0);
        }

        if (greenOverlay != null)
        {
            float gStartPos = (greenStart * totalWidth) - totalWidth / 2f;
            float gWidth = (greenEnd - greenStart) * totalWidth;
            greenOverlay.rectTransform.sizeDelta = new Vector2(gWidth, greenOverlay.rectTransform.sizeDelta.y);
            greenOverlay.rectTransform.anchoredPosition = new Vector2(gStartPos + gWidth / 2, 0);
        }
    }

    private void EvaluateCut()
    {
        float halfWidth = colorBar.rect.width / 2;
        float normalizedX = (arrowIndicator.anchoredPosition.x + halfWidth) / colorBar.rect.width; // 0–1

        string result = "❌ Bad Cut!"; // default red

        if (normalizedX >= yellowStart && normalizedX <= yellowEnd)
            result = "👌 Good Cut!";
        if (normalizedX >= greenStart && normalizedX <= greenEnd)
            result = "✅ Very Good Cut!";

        Debug.Log(result);

        // ✅ Change ingredient to chopped
        if (currentItem != null && currentItem.itemSO != null && currentItem.itemSO.choppedSprite != null)
        {
            Image img = currentItem.GetComponent<Image>();
            if (img != null) img.sprite = currentItem.itemSO.choppedSprite;

            currentItem.SetCutVersion();
            currentItem.EnableDrag();
        }

        cuttingPanel.SetActive(false);
        currentItem = null;
    }
}
