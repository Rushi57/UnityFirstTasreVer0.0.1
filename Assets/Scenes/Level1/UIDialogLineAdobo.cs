using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class UIDialogLineAdobo
{
    [TextArea(2, 5)]
    public string dialogText;
    public Vector2 dialogPosition;
    public string uiElementName;
    public bool waitForClickOnUI;

    [Header("Optional Image")]
    public Image dialogImage; // 👈 assign in Inspector (it will show and vanish automatically)

}
