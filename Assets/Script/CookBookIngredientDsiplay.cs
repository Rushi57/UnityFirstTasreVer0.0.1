using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CookBookIngredientDisplay : MonoBehaviour
{
    [Header("UI References")]
    public Image dishImage;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI regionText;
    public TextMeshProUGUI descriptionText;

    [Header("Ingredient List")]
    public Transform ingredientContainer;
    public GameObject ingredientIconPrefab; // prefab with icon + TMP text

    [Header("CookBook Data")]
    public CookBookSO cookBookSO;

    private void Start()
    {
        if (cookBookSO != null)
            SetupCookBook(cookBookSO);
    }

    public void SetupCookBook(CookBookSO data)
    {
        cookBookSO = data;
        if (data == null)
        {
            Debug.LogWarning("CookBookDisplay: No CookBookSO assigned!");
            return;
        }

        // Set main info
        if (titleText) titleText.text = data.dishName;
        if (regionText) regionText.text = data.region;
        if (descriptionText) descriptionText.text = data.dishDescription;
        if (dishImage) dishImage.sprite = data.dishImage;

        // Clear existing ingredient icons
        foreach (Transform child in ingredientContainer)
            Destroy(child.gameObject);

        // Populate ingredient list
        foreach (var ingredient in data.ingredients)
        {
            if (ingredient == null || ingredient.itemSO == null) continue;

            GameObject iconObj = Instantiate(ingredientIconPrefab, ingredientContainer);
            iconObj.name = $"Ingredient_{ingredient.itemSO.itemName}";

            // Set the image and text
            Image img = iconObj.GetComponentInChildren<Image>(true);
            TextMeshProUGUI txtDesc = iconObj.GetComponentInChildren<TextMeshProUGUI>(true);

            if (img != null && ingredient.itemSO.itemSprite != null)
                img.sprite = ingredient.itemSO.itemSprite;

            if (txtDesc != null)
                txtDesc.text = ingredient.description;
        }
    }
}
