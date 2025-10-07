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
    public GameObject ingredientPrefab; // should have Image + TMP

    [Header("CookBook Data")]
    public CookBookSO cookBookSO;

    private void Start()
    {
        if (cookBookSO != null)
            SetupCookBook(cookBookSO);
    }

    public void SetupCookBook(CookBookSO data)
    {
        if (data == null)
        {
            Debug.LogWarning("❌ No CookBookSO assigned!");
            return;
        }

        // Set main info
        if (titleText) titleText.text = data.dishName;
        if (regionText) regionText.text = data.region;
        if (descriptionText) descriptionText.text = data.dishDescription;
        if (dishImage) dishImage.sprite = data.dishImage;

        // Clear previous list
        foreach (Transform child in ingredientContainer)
            Destroy(child.gameObject);

        // Spawn new ingredients
        foreach (var ingredient in data.ingredients)
        {
            if (ingredient == null || ingredient.itemSO == null) continue;

            GameObject newIngredient = Instantiate(ingredientPrefab, ingredientContainer);
            newIngredient.name = ingredient.itemSO.itemName;

            // 🔹 Get references
            Image img = newIngredient.transform.Find("IngredientImage")?.GetComponent<Image>();
            TextMeshProUGUI txt = newIngredient.transform.Find("IngredientText")?.GetComponent<TextMeshProUGUI>();

            // 🔹 Apply icon (scaled 60x60)
            if (img != null)
            {
                img.sprite = ingredient.itemSO.itemSprite;
                img.rectTransform.sizeDelta = new Vector2(60, 60);
                img.preserveAspect = true;
            }

            // 🔹 Apply text
            if (txt != null)
            {
                txt.text = ingredient.description;
                txt.alignment = TextAlignmentOptions.Left;
                txt.fontSize = 26;
            }
        }
    }
}
