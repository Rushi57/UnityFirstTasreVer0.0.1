using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class Level1TutorialManager : MonoBehaviour
{
    [Header("Tutorial Panels")]
    public GameObject tutorialTopPanel;
    public GameObject tutorialBottomPanel;
    public TextMeshProUGUI topText;
    public TextMeshProUGUI bottomText;
    public Button topContinueButton;
    public Button bottomContinueButton;

    [Header("Gameplay Control")]
    public MonoBehaviour timerScript; // optional - your timer or game logic
    private bool isPaused;

    [System.Serializable]
    public class TutorialStep
    {
        [TextArea(2, 5)]
        public string message;
        public bool useBottom = true;
        public Button waitForUIButton;   // optional: waits for button click
        public GameObject objectToShow;  // optional: show UI or panel (like refrigerator)
    }

    [Header("Tutorial Flow")]
    public List<TutorialStep> tutorialSteps = new List<TutorialStep>();
    private int currentStep = 0;

    private void Start()
    {
        // Hide both panels at start
        tutorialTopPanel.SetActive(false);
        tutorialBottomPanel.SetActive(false);

        // Start tutorial for first-time players
        StartTutorial();
    }

    private void StartTutorial()
    {
        PauseGameplay();
        ShowStep(0);
    }

    private void ShowStep(int index)
    {
        if (index >= tutorialSteps.Count)
        {
            EndTutorial();
            return;
        }

        currentStep = index;
        var step = tutorialSteps[index];

        // Pause the game whenever a tutorial message appears
        PauseGameplay();

        // Hide all panels first
        tutorialTopPanel.SetActive(false);
        tutorialBottomPanel.SetActive(false);

        // Show object if needed (like refrigerator)
        if (step.objectToShow != null)
            step.objectToShow.SetActive(true);

        // Choose panel
        if (step.useBottom)
        {
            tutorialBottomPanel.SetActive(true);
            bottomText.text = step.message;
            bottomContinueButton.onClick.RemoveAllListeners();
            bottomContinueButton.onClick.AddListener(() => OnContinueClicked());
        }
        else
        {
            tutorialTopPanel.SetActive(true);
            topText.text = step.message;
            topContinueButton.onClick.RemoveAllListeners();
            topContinueButton.onClick.AddListener(() => OnContinueClicked());
        }

        // Wait for UI click if assigned
        if (step.waitForUIButton != null)
        {
            step.waitForUIButton.onClick.AddListener(OnUIButtonClicked);
            HighlightUI(step.waitForUIButton.gameObject, true);
        }
    }

    private void OnContinueClicked()
    {
        var step = tutorialSteps[currentStep];

        // Remove any highlight if we were waiting for a button
        if (step.waitForUIButton != null)
            HighlightUI(step.waitForUIButton.gameObject, false);

        // Move to next step
        ShowStep(currentStep + 1);
    }

    private void OnUIButtonClicked()
    {
        var step = tutorialSteps[currentStep];

        // Clean up listener and glow
        if (step.waitForUIButton != null)
        {
            step.waitForUIButton.onClick.RemoveListener(OnUIButtonClicked);
            HighlightUI(step.waitForUIButton.gameObject, false);
        }

        // Proceed
        ShowStep(currentStep + 1);
    }

    private void HighlightUI(GameObject target, bool enable)
    {
        var img = target.GetComponent<Image>();
        if (img == null) return;

        if (enable)
            StartCoroutine(GlowEffect(img));
        else
            StopCoroutine(nameof(GlowEffect));
    }

    private IEnumerator GlowEffect(Image img)
    {
        float speed = 2f;
        Color original = img.color;

        while (true)
        {
            float t = (Mathf.Sin(Time.unscaledTime * speed) + 1f) / 2f;
            img.color = Color.Lerp(original, Color.white, t);
            yield return null;
        }
    }

    private void PauseGameplay()
    {
        if (isPaused) return;
        isPaused = true;
        Time.timeScale = 0f;

        if (timerScript != null)
            timerScript.enabled = false;
    }

    private void ResumeGameplay()
    {
        if (!isPaused) return;
        isPaused = false;
        Time.timeScale = 1f;

        if (timerScript != null)
            timerScript.enabled = true;
    }

    private void EndTutorial()
    {
        tutorialTopPanel.SetActive(false);
        tutorialBottomPanel.SetActive(false);
        ResumeGameplay();
    }
}
