using UnityEngine;

public class GlowManager : MonoBehaviour
{
    public static GlowManager Instance;

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
