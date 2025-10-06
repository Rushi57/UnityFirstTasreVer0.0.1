using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CookBookItemData : MonoBehaviour
{
    public ItemSO itemSO;

    [Header("UI Reference")]
    public Image iconImage;
    public TextMeshProUGUI descriptionText;

    public void SetupItem(ItemSO data, string description)
    {
        itemSO = data;

        if (iconImage && data.itemSprite)
            iconImage.sprite = data.itemSprite;

        if (descriptionText)
            descriptionText.text = description;
    }
}
