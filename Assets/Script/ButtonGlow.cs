using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class GlowButton : MonoBehaviour
{
    public Color glowColor = Color.yellow;
    public float pulseSpeed = 2f;

    private Image image;
    private Color originalColor;
    private bool isGlowing = false;

    void Awake()
    {
        image = GetComponent<Image>();
        originalColor = image.color;
    }

    void Update()
    {
        if (isGlowing)
        {
            float t = (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f;
            image.color = Color.Lerp(originalColor, glowColor, t);
        }
    }

    public void StartGlow()
    {
        isGlowing = true;
    }

    public void StopGlow()
    {
        isGlowing = false;
        image.color = originalColor;
    }

    // Call this in Button OnClick
    public void OnClick()
    {
        StopGlow(); // stops glow when clicked
    }
}
