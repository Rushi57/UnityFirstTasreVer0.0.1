using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class UITutorialManager : MonoBehaviour
{
    [Header("Dialog Settings")]
    public UIDialogLine[] dialogLines;

    [Header("Canvas References")]
    public GameObject dialogCanvas;
    public GameObject mapMainCanvas;
    public GameObject settingCanvas;

    [Header("UI Elements")]
    public TextMeshProUGUI dialogText;
    public Button dialogBoxButton;
    public GameObject dialogPanel;
    private int currentLine = 0;
    private bool waitingForClick = false;
    private bool readyToAdvance = false;

    void Start()
    {
        // Skip tutorial if already finished
        if (PlayerPrefs.GetInt("TutorialDone", 0) == 1)
        {
            if (dialogCanvas) dialogCanvas.SetActive(false);
            return;
        }

        if (dialogBoxButton != null)
            dialogBoxButton.onClick.AddListener(OnDialogBoxClicked);

        StartCoroutine(PlayTutorial());
    }

    IEnumerator PlayTutorial()
    {
        if (dialogCanvas) dialogCanvas.SetActive(true);
        if (mapMainCanvas) mapMainCanvas.SetActive(true);
        if (settingCanvas) settingCanvas.SetActive(false);

        while (currentLine < dialogLines.Length)
        {
            UIDialogLine line = dialogLines[currentLine];
            yield return StartCoroutine(TypeText(line.dialogText));
            readyToAdvance = false;

            // 🔍 Find the UI element to interact with
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

            // 🟧 If this line asks to click something
            if (line.waitForClickOnUI && buttonToHighlight != null)
            {
                // Disable blocking panel so player can click
                if (dialogPanel != null)
                    dialogPanel.SetActive(false);

                waitingForClick = true;
                buttonToHighlight.onClick.AddListener(() => OnUIElementClicked(buttonToHighlight));

                // Wait until player clicks
                yield return new WaitUntil(() => !waitingForClick);

                // Re-enable blocking panel afterward
                if (dialogPanel != null)
                    dialogPanel.SetActive(true);
            }
            else
            {
                // Keep dialog panel blocking while reading
                if (dialogPanel != null)
                    dialogPanel.SetActive(true);

                yield return new WaitUntil(() => readyToAdvance);
            }

            currentLine++;
        }

        // When tutorial ends
        if (dialogCanvas) dialogCanvas.SetActive(false);
        PlayerPrefs.SetInt("TutorialDone", 1);
        PlayerPrefs.Save();
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

        // ✅ Close the dialog box
        if (dialogCanvas != null)
            dialogCanvas.SetActive(false);
        
        // ✅ Open the settings panel when SettingButton is clicked
        if (button.name == "SettingButton" && settingCanvas != null)
            settingCanvas.SetActive(true);
        if(dialogPanel == null)
            dialogPanel.SetActive(false);
        // Wait a short moment, then show the next dialog (after settings open)
        StartCoroutine(ShowNextDialogAfterDelay(1f));
    }

    IEnumerator ShowNextDialogAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Reopen the dialog box to show the next tutorial message
        if (dialogCanvas != null)
            dialogCanvas.SetActive(true);
    }

    void OnDialogBoxClicked()
    {
        if (!waitingForClick)
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
