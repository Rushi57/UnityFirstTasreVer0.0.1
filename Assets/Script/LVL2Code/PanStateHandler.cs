using UnityEngine;
using UnityEngine.UI;

public class PanStateHandler : MonoBehaviour
{
    public Image panImage;           // assign in prefab inspector
    public Sprite defaultPanSprite;  // empty pan
    public Sprite[] stepSprites;     // pan + ingredient versions

    private int stepIndex = 0;

    public void UpdatePan(ItemSO itemSO)
    {
        if (stepSprites.Length > stepIndex)
        {
            panImage.sprite = stepSprites[stepIndex];
            stepIndex++;
        }
        else
        {
            Debug.LogWarning("⚠️ No more pan states available.");
        }
    }
}
