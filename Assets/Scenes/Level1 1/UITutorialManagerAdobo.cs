using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using TMPro;

public class UITutorialManagerAdobo : MonoBehaviour
{
    [Header("Dialog Settings")]
    public UIDialogLineAdobo[] dialogLines;

    [Header("Canvas References")]
    public GameObject dialogCanvasAdobo;
    public GameObject mapMainCanvasAdobo;

    [Header("UI Elements")]
    public TextMeshProUGUI dialogText;
    public Button dialogBoxButton;

    private int currentLine = 0;
    private bool waitingForClick = false;
    private bool readyToAdvance = false;
    private bool waitingForResumeButton = false;

    private Image currentImage = null;

    // Keep track of listeners we add so we can remove them cleanly
    private Dictionary<Button, UnityAction> addedResumeListeners = new Dictionary<Button, UnityAction>();

    void Start()
    {
        // Skip tutorial if already finished
        if (PlayerPrefs.GetInt("TutorialAdobo", 0) == 2)
        {
            if (dialogCanvasAdobo) dialogCanvasAdobo.SetActive(false);
            return;
        }

        if (dialogBoxButton != null)
            dialogBoxButton.onClick.AddListener(OnDialogBoxClicked);

        StartCoroutine(PlayTutorial());
    }

    IEnumerator PlayTutorial()
    {
        if (dialogCanvasAdobo) dialogCanvasAdobo.SetActive(true);
        if (mapMainCanvasAdobo) mapMainCanvasAdobo.SetActive(true);

        while (currentLine < dialogLines.Length)
        {
            UIDialogLineAdobo line = dialogLines[currentLine];

            // Move dialog box
            if (dialogCanvasAdobo != null)
            {
                RectTransform rect = dialogCanvasAdobo.GetComponent<RectTransform>();
                if (rect != null)
                    rect.anchoredPosition = line.dialogPosition;
            }

            // Show image (if any)
            if (line.dialogImage != null)
            {
                currentImage = line.dialogImage;
                currentImage.gameObject.SetActive(true);
            }

            yield return StartCoroutine(TypeText(line.dialogText));
            readyToAdvance = false;

            // Setup a UI element highlight / wait for it if requested
            Button buttonToHighlight = null;
            if (!string.IsNullOrEmpty(line.uiElementName))
            {
                GameObject uiObj = GameObject.Find(line.uiElementName);
                if (uiObj != null)
                {
                    buttonToHighlight = uiObj.GetComponent<Button>();
                    HighlightUI(uiObj);
                }
            }

            // Wait for UI click (existing behavior)
            if (line.waitForClickOnUI && buttonToHighlight != null)
            {
                waitingForClick = true;
                buttonToHighlight.onClick.RemoveAllListeners();
                buttonToHighlight.onClick.AddListener(() => OnUIElementClicked(buttonToHighlight));
                yield return new WaitUntil(() => !waitingForClick);
            }
            // Wait for resume trigger button (new behavior)
            else if (line.waitForResumeButton)
            {
                // Hide dialogue while player does the mechanic
                if (dialogCanvasAdobo != null)
                    dialogCanvasAdobo.SetActive(false);

                waitingForResumeButton = true;

                // Prefer inspector-assigned Button reference
                Button resumeBtn = line.resumeButton;

                // Fallback to name-based search if no direct reference provided
                if (resumeBtn == null && !string.IsNullOrEmpty(line.resumeButtonName))
                {
                    GameObject resumeObj = GameObject.Find(line.resumeButtonName);
                    if (resumeObj != null)
                        resumeBtn = resumeObj.GetComponent<Button>();
                }

                // If we still don't have the button, start a short polling coroutine to wait until it exists/activates
                if (resumeBtn == null && !string.IsNullOrEmpty(line.resumeButtonName))
                {
                    yield return StartCoroutine(WaitForResumeButtonByName(line.resumeButtonName, (b) =>
                    {
                        // callback when found
                        RegisterResumeListener(b);
                    }));
                }
                else if (resumeBtn != null)
                {
                    RegisterResumeListener(resumeBtn);
                }
                else
                {
                    // No resume button specified — just continue (avoid blocking forever)
                    Debug.LogWarning("[UITutorialManagerAdobo] waitForResumeButton is true but no resume button was found/assigned for line " + currentLine);
                    waitingForResumeButton = false;
                }

                // Wait until the resume callback clears waitingForResumeButton
                yield return new WaitUntil(() => !waitingForResumeButton);
            }
            else
            {
                yield return new WaitUntil(() => readyToAdvance);
            }

            // Hide image when advancing
            if (currentImage != null)
            {
                currentImage.gameObject.SetActive(false);
                currentImage = null;
            }

            currentLine++;
        }

        // End tutorial
        if (dialogCanvasAdobo) dialogCanvasAdobo.SetActive(false);
        PlayerPrefs.SetInt("TutorialAdobo", 1);
        PlayerPrefs.Save();
    }

    // If the resume button isn't present at the time we need it, wait until it's created/active
    IEnumerator WaitForResumeButtonByName(string buttonName, System.Action<Button> onFound)
    {
        if (string.IsNullOrEmpty(buttonName))
        {
            yield break;
        }

        Button found = null;
        // keep trying each frame until found (or you can add a timeout if you want)
        while (found == null)
        {
            GameObject obj = GameObject.Find(buttonName); // NOTE: GameObject.Find won't find inactive objects
            if (obj != null)
                found = obj.GetComponent<Button>();

            if (found != null)
                break;

            yield return null;
        }

        onFound?.Invoke(found);
    }

    void RegisterResumeListener(Button resumeBtn)
    {
        if (resumeBtn == null) return;

        // Remove an old listener we previously added to avoid duplication
        if (addedResumeListeners.ContainsKey(resumeBtn))
        {
            resumeBtn.onClick.RemoveListener(addedResumeListeners[resumeBtn]);
            addedResumeListeners.Remove(resumeBtn);
        }

        UnityAction action = () => OnResumeButtonClicked(resumeBtn);
        resumeBtn.onClick.AddListener(action);
        addedResumeListeners[resumeBtn] = action;
    }

    void HighlightUI(GameObject ui)
    {
        var image = ui.GetComponent<Image>();
        if (image != null)
            image.color = Color.white;
    }

    void OnUIElementClicked(Button button)
    {
        waitingForClick = false;
        button.onClick.RemoveAllListeners();
        readyToAdvance = true;
    }

    void OnResumeButtonClicked(Button button)
    {
        // Remove only the listener we added
        if (button != null && addedResumeListeners.ContainsKey(button))
        {
            button.onClick.RemoveListener(addedResumeListeners[button]);
            addedResumeListeners.Remove(button);
        }

        waitingForResumeButton = false;

        // Show dialogue again
        if (dialogCanvasAdobo != null)
            dialogCanvasAdobo.SetActive(true);

        readyToAdvance = true;
    }

    void OnDialogBoxClicked()
    {
        if (!waitingForClick && !waitingForResumeButton)
            readyToAdvance = true;
    }

    IEnumerator TypeText(string textToType)
    {
        dialogText.text = "";
        foreach (char c in textToType)
        {
            dialogText.text += c;
            yield return new WaitForSeconds(0.02f);
        }
    }
    
}
