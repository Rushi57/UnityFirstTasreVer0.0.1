using UnityEngine;

[System.Serializable]
public class UIDialogLineAdobo
{
    [TextArea(2, 5)]
    public string dialogText; // The text shown in the dialog

    public string uiElementName; // Name of the UI element to highlight
    public bool waitForClickOnUI; // Should wait for UI click before advancing

    // 👇 New field: controls where the dialog box appears
    public Vector3 dialogPosition;
}
