using UnityEngine;
using UnityEngine.UI;

public class PanStateHandler : MonoBehaviour
{
    public Image panImage;           // assign in Inspector
    public Sprite defaultPanSprite;

    [Header("Ingredient States (ordered)")]
    public Sprite[] stepSprites;     // one sprite per ingredient step (in the order ingredients are added)

    [Header("Condiment (pour) Sprites")]
    public Sprite oilPanSprite;
    public Sprite vinegarPanSprite;
    public Sprite soyPanSprite;

    private int stepIndex = 0;

    // Call when an ingredient is accepted
    public void UpdatePan(ItemSO itemSO)
    {
        if (stepSprites == null || stepSprites.Length == 0)
        {
            Debug.LogWarning("PanStateHandler: no stepSprites assigned.");
            return;
        }

        if (stepIndex < stepSprites.Length)
        {
            panImage.sprite = stepSprites[stepIndex];
            stepIndex++;
        }
        else
        {
            Debug.LogWarning("PanStateHandler: no more ingredient sprites available.");
        }
    }

    // Call when a condiment action (Oil/Vinegar/Soy) is accepted
    public void UpdatePanWithAction(string action)
    {
        if (string.IsNullOrEmpty(action)) return;

        switch (action.Trim().ToLower())
        {
            case "oil":
                if (oilPanSprite != null) panImage.sprite = oilPanSprite;
                break;
            case "vinegar":
                if (vinegarPanSprite != null) panImage.sprite = vinegarPanSprite;
                break;
            case "soy":
            case "soy sauce":
                if (soyPanSprite != null) panImage.sprite = soyPanSprite;
                break;
            default:
                Debug.LogWarning("PanStateHandler: unknown action " + action);
                break;
        }
    }

    // Reset to default pan
    public void ResetPan()
    {
        panImage.sprite = defaultPanSprite;
        stepIndex = 0;
    }
}
