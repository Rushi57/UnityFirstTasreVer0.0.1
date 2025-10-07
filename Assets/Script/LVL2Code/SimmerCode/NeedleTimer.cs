using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class NeedleTimer : MonoBehaviour
{
    public RectTransform needle;
    public float speed = 180f;
    public bool rotating = true;

    private float rotationTime;
    private float fullRotationTime;

    [Header("Sectors (degrees)")]
    public float yellowStart = 330f;
    public float yellowEnd = 20f;
    public float greenStart = 20f;
    public float greenEnd = 60f;

    [Header("Panels & UI")]
    public GameObject simmerPanel;
    public GameObject completeShowPanel;
    public TextMeshProUGUI resultText;
    public Button closeButton;

    // 🔹 Temp storage for score & result
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
            {
                AutoFail();
            }
        }
    }

    public void StopNeedle()
    {
        if (!rotating) return;

        rotating = false;

        float angle = needle.eulerAngles.z;
        angle = (360f - angle + 90f) % 360f;

        pendingResult = "Bad";
        pendingScore = 3; // default

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

        // Show result panel (but no score yet)
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

        // Always Bad
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

    private void ClosePanels()
    {
        TotalScoreManager.Instance.AddSimmerScore(pendingScore);
        StartCoroutine(CloseAndProceed());
    }

    private IEnumerator CloseAndProceed()
    {
        if (completeShowPanel != null)
            completeShowPanel.SetActive(false);

        if (simmerPanel != null)
            simmerPanel.SetActive(false);

        yield return new WaitForSeconds(0.3f);
        CookingStepManager.Instance.NextStep();
    }
}
