using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class UIDialogLineAdobo
{
    public string dialogText;
    public Vector2 dialogPosition;
    public bool waitForClickOnUI;
    public string uiElementName;
    public Image dialogImage;

    [Header("Resume Control")]
    public bool waitForResumeButton;      // Should pause here until a button is clicked?
    public Button resumeButton;           // Assign the Button directly in the Inspector
    public string resumeButtonName;       // optional fallback (keeps backwards compatibility)

    
}

