using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CuttingMechanic : MonoBehaviour
{
    [Header("UI References")]
    public GameObject cuttingPanel;
    public Image ingredientDisplayImg;
    public Button tapToCutBtn;
    public RectTransform arrowIndicator;
    public RectTransform colorBar; // the single white bar

    [Header("Completion Panel UI")]
    public GameObject completeshowPanel;
    public Image cutVersionImg;
    public TextMeshProUGUI resultText;
    public Button closeButton;

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

    // ✅ New flag to prevent multiple cuts
    private bool hasCut = false;

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
        if (hasCut) return; // ✅ stop movement after cutting

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
        hasCut = false; // ✅ reset when new item starts
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

        float yellowWidth = 0.5f;
        yellowStart = Random.Range(0f, 1f - yellowWidth);
        yellowEnd = yellowStart + yellowWidth;

        float greenWidth = 0.1f;
        greenStart = Random.Range(yellowStart, yellowEnd - greenWidth);
        greenEnd = greenStart + greenWidth;

        Debug.Log($"Zones → Yellow: {yellowStart:P0}-{yellowEnd:P0}, Green: {greenStart:P0}-{greenEnd:P0}");

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
        if (hasCut) return; // ✅ Prevent multiple cuts on same item
        hasCut = true;

        float halfWidth = colorBar.rect.width / 2;
        float normalizedX = (arrowIndicator.anchoredPosition.x + halfWidth) / colorBar.rect.width;

        string result = "Bad Cut!";
        int scoreChopped = 1;

        if (normalizedX >= yellowStart && normalizedX <= yellowEnd)
        {
            result = "Good Cut!";
            scoreChopped = 2;
        }
        if (normalizedX >= greenStart && normalizedX <= greenEnd)
        {
            result = "Very Good Cut!";
            scoreChopped = 3;
        }

        // Send Score
      
        TotalScoreManager.Instance.AddCutScore(scoreChopped);

        // Show result panel
        Sprite choppedSprite = null;
        if (currentItem != null && currentItem.itemSO != null)
            choppedSprite = currentItem.itemSO.choppedSprite;

        ShowResultPanel(result, choppedSprite);

        // ✅ Change ingredient to chopped but still draggable
        if (currentItem != null && currentItem.itemSO != null && currentItem.itemSO.choppedSprite != null)
        {
            Image img = currentItem.GetComponent<Image>();
            if (img != null) img.sprite = currentItem.itemSO.choppedSprite;

            currentItem.SetCutVersion();
            currentItem.EnableDrag();
        }
    }

    private void ShowResultPanel(string result, Sprite choppedSprite)
    {
        if (completeshowPanel == null || resultText == null) return;

        completeshowPanel.SetActive(true);
        resultText.text = result;

        if (cutVersionImg != null && choppedSprite != null)
            cutVersionImg.sprite = choppedSprite;
    }

   
}
