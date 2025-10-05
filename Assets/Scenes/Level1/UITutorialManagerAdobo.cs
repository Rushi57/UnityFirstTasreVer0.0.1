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

            // Wait for user to click the highlighted UI
            if (line.waitForClickOnUI && buttonToHighlight != null)
            {
                waitingForClick = true;
                buttonToHighlight.onClick.AddListener(() => OnUIElementClicked(buttonToHighlight));
                yield return new WaitUntil(() => !waitingForClick);
            }
            else
            {
                yield return new WaitUntil(() => readyToAdvance);
            }

            currentLine++;
        }

        // When tutorial ends
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

        // ✅ Close the dialog box
        if (dialogCanvasAdobo != null)
            dialogCanvasAdobo.SetActive(false);



        // Wait a short moment, then show the next dialog (after settings open)
        StartCoroutine(ShowNextDialogAfterDelay(1f));
    }

    IEnumerator ShowNextDialogAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Reopen the dialog box to show the next tutorial message
        if (dialogCanvasAdobo != null)
            dialogCanvasAdobo.SetActive(true);
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
