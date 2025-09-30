using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class DishDisplayUI : MonoBehaviour
{
    [Header("UI References")]
    public GameObject panel;
    public Image dishImage;
    public TextMeshProUGUI dishTitle;
    public TextMeshProUGUI message;

    private void Start()
    {
        if (panel != null)
            panel.SetActive(false); // only hide panel visuals, not the script's GameObject
    }

    public void ShowDish(RecipeSO recipe)
    {
        if (recipe == null) return;

        panel.SetActive(true);
        dishImage.sprite = recipe.recipeImage;
        dishTitle.text = recipe.recipeName;
        message.text = $"🎉 Congratulations! You finished cooking {recipe.recipeName}!";

      
    }

    public void ShowDishWithDelay(RecipeSO recipe, float delay)
    {
        StartCoroutine(ShowDishCoroutine(recipe, delay));
    }

    private IEnumerator ShowDishCoroutine(RecipeSO recipe, float delay)
    {
        yield return new WaitForSeconds(delay);
        ShowDish(recipe);
    }
}
