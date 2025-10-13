using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Level1UIDialogLine : MonoBehaviour
{
    [Header("Top Dialog")]
    public GameObject topPanel;
    public Image topCharacterImage;
    public TMP_Text topDialogText;

    [Header("Bottom Dialog")]
    public GameObject bottomPanel;
    public Image bottomCharacterImage;
    public TMP_Text bottomDialogText;

    public void ShowTop(Sprite character, string text)
    {
        topPanel.SetActive(true);
        bottomPanel.SetActive(false);
        topCharacterImage.sprite = character;
        topDialogText.text = text;
    }

    public void ShowBottom(Sprite character, string text)
    {
        topPanel.SetActive(false);
        bottomPanel.SetActive(true);
        bottomCharacterImage.sprite = character;
        bottomDialogText.text = text;
    }

    public void HideAll()
    {
        topPanel.SetActive(false);
        bottomPanel.SetActive(false);
    }
}
