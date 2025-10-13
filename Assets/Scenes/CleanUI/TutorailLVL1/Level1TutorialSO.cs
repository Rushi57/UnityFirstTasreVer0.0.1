using UnityEngine;

[System.Serializable]
public class Level1TutorialLine
{
    [Header("Dialog")]
    [TextArea(2, 4)] public string dialogText;
    public bool showTop; // true = Top dialog box, false = Bottom

    [Header("UI Interaction")]
    public string uiElementName; // The button's name to highlight
    public bool waitForClickOnUI; // Wait for player to click this

    [Header("Gameplay Control")]
    public bool pauseGameplay; // Stop timers, etc.
    public bool resumeGameplay; // Resume gameplay
}
