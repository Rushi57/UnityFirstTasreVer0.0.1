using UnityEngine;
using UnityEngine.UI;
using System.Collections;
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

    private Image currentImage = null;

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

            // Wait for user interaction
            if (line.waitForClickOnUI && buttonToHighlight != null)
            {
                waitingForClick = true;
                buttonToHighlight.onClick.RemoveAllListeners();
                buttonToHighlight.onClick.AddListener(() => OnUIElementClicked(buttonToHighlight));
                yield return new WaitUntil(() => !waitingForClick);
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

        // Hide the dialog canvas briefly
        if (dialogCanvasAdobo != null)
            dialogCanvasAdobo.SetActive(false);

        // After a short delay, bring the dialogue back and continue
        StartCoroutine(ShowNextDialogAfterDelay(1f));
    }

    IEnumerator ShowNextDialogAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (dialogCanvasAdobo != null)
            dialogCanvasAdobo.SetActive(true);

        // Allow dialogue to continue
        readyToAdvance = true;
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
