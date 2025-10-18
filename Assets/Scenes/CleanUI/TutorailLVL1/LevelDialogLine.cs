using UnityEngine;
[System.Serializable]
public class LevelDialogLine
{
    [TextArea(2, 5)]
    public string dialogText;

    [Header("UI Target")]
    public string uiElementName;
    public bool waitForClickOnUi;

    [Header("Panel Display")]
    public bool useTopPanel;
}
