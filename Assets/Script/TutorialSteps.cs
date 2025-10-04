using UnityEngine;

public class TutorialSteps : MonoBehaviour
{
    public GlowButton button1;
    public GlowButton button2;
    public GlowButton button3;

    void Start()
    {
        // Start the tutorial: first button glows
        GlowManager.Instance.ActivateGlow(button1);
    }

    public void OnButton1Clicked()
    {
        // After Button1 is clicked, make Button2 glow
        GlowManager.Instance.ActivateGlow(button2);
    }

    public void OnButton2Clicked()
    {
        // After Button2 is clicked, make Button3 glow
        GlowManager.Instance.ActivateGlow(button3);
    }

    public void NeedButton1Again()
    {
        // Later in tutorial, bring back Button1 glow
        GlowManager.Instance.ActivateGlow(button1);
    }
}
