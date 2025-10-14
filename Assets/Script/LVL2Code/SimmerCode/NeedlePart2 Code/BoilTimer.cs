using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BoilTimer : MonoBehaviour
{
    public RectTransform needle;
    public float speed = 180f;
    public bool rotating = false;

    private float rotationTime;
    private float fullRotationTime;

    [Header("Sectors (degrees)")]
    public float yellowStart = 330f;
    public float yellowEnd = 20f;
    public float greenStart = 20f;
    public float greenEnd = 60f;

    [Header("Panels & UI")]
    public GameObject boilPanel;
    public GameObject completeShowPanel;
    public TextMeshProUGUI resultText;
    public Button closeButton;

    public GameObject setSidePanel;

    private int pendingScore = 0;
    private string pendingResult = "Bad";

    void Start()
    {
        if (closeButton != null)
            closeButton.onClick.AddListener(ClosePanels);

        fullRotationTime = 360f / speed;
    }

    void Update()
    {
        if (rotating)
        {
            needle.Rotate(0f, 0f, -speed * Time.deltaTime);
            rotationTime += Time.deltaTime;

            if (rotationTime >= fullRotationTime)
                AutoFail();
        }
    }

    public void StopNeedle()
    {
        if (!rotating) return;

        rotating = false;

        float angle = needle.eulerAngles.z;
        angle = (360f - angle + 90f) % 360f;

        pendingResult = "Bad";
        pendingScore = 3;

        if (IsAngleInSector(angle, greenStart, greenEnd))
        {
            pendingResult = "Very Good";
            pendingScore = 10;
        }
        else if (IsAngleInSector(angle, yellowStart, yellowEnd))
        {
            pendingResult = "Good";
            pendingScore = 8;
        }

        if (completeShowPanel != null)
        {
            completeShowPanel.SetActive(true);
            if (resultText != null)
                resultText.text = pendingResult;
        }

        Debug.Log($"Angle {angle:F1}° → {pendingResult}");
    }

    bool IsAngleInSector(float angle, float start, float end)
    {
        if (start < end)
            return angle >= start && angle <= end;
        else
            return angle >= start || angle <= end;
    }

    private void AutoFail()
    {
        rotating = false;
        rotationTime = 0f;
        pendingScore = 3;
        pendingResult = "Bad";

        if (completeShowPanel != null)
        {
            completeShowPanel.SetActive(true);
            if (resultText != null)
                resultText.text = "Bad";
        }

        Debug.Log("Auto Fail → Bad (time ran out)");
    }

    public void ClosePanels()
    {
        Debug.Log("[BoilTimer] Close button pressed");

        // ✅ Add simmer/boil score only
        if (TotalScoreManager.Instance != null)
            TotalScoreManager.Instance.AddSimmerScore(pendingScore);
        else
            Debug.LogWarning("[BoilTimer] TotalScoreManager.Instance is NULL!");

        // ✅ Hide the result panel
        if (completeShowPanel != null)
            completeShowPanel.SetActive(false);

        // ✅ Hide the boil gameplay panel
        if (boilPanel != null)
            boilPanel.SetActive(false);

        // ✅ Show Set Aside button (the handler will call NextStep)
        if (setSidePanel != null)
        {
            setSidePanel.gameObject.SetActive(true);
            Debug.Log("[BoilTimer] SetAsideButton shown. Player must click to continue.");
        }
    }

    public void RestartBoil()
    {
        Debug.Log("[BoilTimer] Restarting boil mini-game...");

        rotationTime = 0f;
        rotating = true;

        if (needle != null)
            needle.localRotation = Quaternion.Euler(0f, 0f, 90f);

        if (completeShowPanel != null)
            completeShowPanel.SetActive(false);

        if (boilPanel != null)
            boilPanel.SetActive(true);

        if (setSidePanel != null)
            setSidePanel.gameObject.SetActive(true); // ✅ Hides until ClosePanels() is called

        pendingScore = 0;
        pendingResult = "Bad";

        Debug.Log("[BoilTimer] Boil reset and started.");
    }
}
