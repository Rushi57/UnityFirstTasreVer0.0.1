using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(CanvasScaler))]
public class AutoCanvasAdjuster : MonoBehaviour
{
    [Tooltip("Base Resolution used in UI Design")]
    public Vector2 referenceResolution = new Vector2(1280, 720);

    [Tooltip("Auto Adjust for screenSize")]
    [Range(0,1)] public float matchFactor = 0.5f;

    private CanvasScaler scaler;

    void Start()
    {
        scaler = GetComponent<CanvasScaler>();
        AdjustCanvas();
    }

    void AdjustCanvas()
    {
        if (scaler == null)
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = referenceResolution;
        scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;

        float currentAspect = (float)Screen.width / Screen.height;
        float baseAspect = referenceResolution.x / referenceResolution.y;

        scaler.matchWidthOrHeight = currentAspect > baseAspect ? 1f : 0f;

        scaler.matchWidthOrHeight = Mathf.Lerp(0f, 1f, matchFactor);
    }

}
