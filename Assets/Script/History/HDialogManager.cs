using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class HDialogManager : MonoBehaviour
{
    [SerializeField]
    public HdialogData HdialogsData;
    public TextMeshProUGUI dialogText;
    public Image dishImage;
    public Button dialogButton;
    public GameObject dialogHistoryCanvas;
    public float typeSpeed = 0.03f;
  

    private int currentLine = 0;
    private Coroutine typingCoroutine;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dialogButton.onClick.AddListener(NextLine);
        currentLine = 0;

        // Set dish sprite once
        if (dishImage != null && HdialogsData.dishSprite != null)
            dishImage.sprite = HdialogsData.dishSprite;

        ShowLine();

    }

    // Update is called once per frame
    void ShowLine()
    {
        if (HdialogsData != null && currentLine < HdialogsData.hDialogLines.Length)
        {
            // Stop typing if already typing
            if (typingCoroutine != null) StopCoroutine(typingCoroutine);

            string line = HdialogsData.hDialogLines[currentLine].text;
            typingCoroutine = StartCoroutine(TypeText(line));

        }
        else
        {
            dialogText.text = "";
            return;
           
        }
    }
    IEnumerator TypeText(string line)
    {
        dialogText.text = "";
        foreach (char c in line)
        {
            dialogText.text += c;
            yield return new WaitForSeconds(typeSpeed);
        }
    }
    void NextLine()
    {
        if (typingCoroutine != null && dialogText.text != HdialogsData.hDialogLines[currentLine].text)
        {
            //Skip typing and instatly show full line 
            StopCoroutine(typingCoroutine);
            dialogText.text = HdialogsData.hDialogLines[currentLine].text;
            return;
        }
        currentLine++;

        if(currentLine >=  HdialogsData.hDialogLines.Length)
        {
            dialogHistoryCanvas.SetActive(false);
            return;
        }
        ShowLine();
    }
}
