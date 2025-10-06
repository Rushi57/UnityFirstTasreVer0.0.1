// IngredientIcon.cs
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IngredientIcon : MonoBehaviour
{
    [Header("UI refs (assign in prefab)")]
    public Image iconImage;
    public TextMeshProUGUI label;

    /// <summary>
    /// Fill icon + text and optionally adjust text offset.
    /// </summary>
    public void Setup(Sprite sprite, string text, Vector2 textOffset)
    {
        if (iconImage != null)
            iconImage.sprite = sprite;

        if (label != null)
        {
            label.text = text;
            RectTransform tr = label.GetComponent<RectTransform>();
            if (tr != null)
                tr.anchoredPosition += textOffset;
        }
    }
}
