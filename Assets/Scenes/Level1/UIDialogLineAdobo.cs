using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "NewDialogLine", menuName = "Tutorial/Tutorial Line")]
public class UIDialogLineAdobo : ScriptableObject
{
    [TextArea]
    public string dialogText;

    public string uiElementName;     //UnityEngine.UI.Button
    public bool waitForClickOnUI;
}
