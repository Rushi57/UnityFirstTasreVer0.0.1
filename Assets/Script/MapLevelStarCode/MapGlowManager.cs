using UnityEngine;

public class MapGlowManager : MonoBehaviour
{
    public static MapGlowManager Instance;

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void ActivateGlow(GlowButton button)
    {
        if (button != null)
        {
            button.StartGlow();
        }
    }
}
