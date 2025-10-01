// InfoPanelController.cs
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class InfoPanelController : MonoBehaviour
{
    public Image dishImage;
    public TextMeshProUGUI titleText;
    public Button playButton;
    public Button closeButton;

    public void Setup(RecipeSO recipe, UnityAction onPlay)
    {
        if (dishImage != null) dishImage.sprite = recipe.recipeImage;
        if (titleText != null) titleText.text = recipe.recipeName;

        if (playButton != null)
        {
            playButton.onClick.RemoveAllListeners();
            playButton.onClick.AddListener(onPlay);
        }

        if (closeButton != null)
        {
            closeButton.onClick.RemoveAllListeners();
            closeButton.onClick.AddListener(() => Destroy(gameObject));
        }
    }
}
