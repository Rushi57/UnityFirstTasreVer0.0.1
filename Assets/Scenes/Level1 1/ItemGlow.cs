using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ItemGlow : MonoBehaviour
{
    [Header("Glow Settings")]
    public Color glowColor = Color.yellow;
    [Range(0f, 3f)] public float glowIntensity = 1.5f;
    [Range(0.1f, 5f)] public float pulseSpeed = 2f;
    [HideInInspector] public bool glowing = false;

    private Image image;
    private Material instanceMaterial;
    private Color baseColor;

    void Awake()
    {
        image = GetComponent<Image>();
        baseColor = image.color;

        // Clone a material instance so other UI images aren't affected
        instanceMaterial = new Material(Shader.Find("UI/Default"));
        instanceMaterial.color = baseColor;
        image.material = instanceMaterial;
    }

    void Update()
    {
        if (glowing)
        {
            float pulse = (Mathf.Sin(Time.time * pulseSpeed) + 1f) * 0.5f; // 0..1
            Color blended = Color.Lerp(baseColor, glowColor, pulse);
            // apply intensity (clamped)
            blended.r = Mathf.Clamp01(blended.r * (1f + glowIntensity));
            blended.g = Mathf.Clamp01(blended.g * (1f + glowIntensity));
            blended.b = Mathf.Clamp01(blended.b * (1f + glowIntensity));
            blended.a = baseColor.a;
            instanceMaterial.color = blended;
        }
        else
        {
            instanceMaterial.color = baseColor;
        }
    }

    public void StartGlow() => glowing = true;
    public void StopGlow() => glowing = false;
}
