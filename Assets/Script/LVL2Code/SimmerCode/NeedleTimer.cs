using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NeedleTimer : MonoBehaviour
{
    public RectTransform needle;   // drag your needle here
    public float speed = 180f;     // degrees per second
    public bool rotating = true;

    private float rotationTime;    // tracks how long it's been rotating
    private float fullRotationTime; // time to complete one full circle

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

    void Start()
    {
        if (closeButton != null)
            closeButton.onClick.AddListener(ClosePanels);

        // how many seconds for a full 360 rotation
        fullRotationTime = 360f / speed;
    }

    void Update()
    {
        if (rotating)
        {
            needle.Rotate(0f, 0f, -speed * Time.deltaTime);
            rotationTime += Time.deltaTime;

            // ✅ auto stop after one full rotation
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

        string result = "Bad";
        int simmerscore = 3; // default Bad

        if (IsAngleInSector(angle, greenStart, greenEnd))
        {
            result = "Very Good";
            simmerscore = 10;
        }
        else if (IsAngleInSector(angle, yellowStart, yellowEnd))
        {
            result = "Good";
            simmerscore = 8;
        }

        // Add score
        TotalScoreManager.Instance.AddSimmerScore(simmerscore);
        CookingStepManager.Instance.NextStep();

        // Show result panel
        if (completeShowPanel != null)
        {
            completeShowPanel.SetActive(true);
            if (resultText != null)
                resultText.text = result;
        }

        Debug.Log($"Angle {angle:F1}° → {result}");
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

        // ✅ Always Bad if time runs out
        TotalScoreManager.Instance.AddSimmerScore(3);

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
        if (completeShowPanel != null)
            completeShowPanel.SetActive(false);

        if (simmerPanel != null)
            simmerPanel.SetActive(false);
    }
}
