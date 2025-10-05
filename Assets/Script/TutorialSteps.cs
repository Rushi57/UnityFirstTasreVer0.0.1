using UnityEngine;

public class TutorialSteps : MonoBehaviour
{
    public GlowButton[] buttons; // Assign 17 buttons in Inspector
    private int currentIndex = 0;

    void Start()
    {
        // Start tutorial with first button glowing
        GlowManager.Instance.ActivateGlow(buttons[0]);
    }

    public void OnButtonClicked(int index)
    {
        if (index < buttons.Length - 1)
        {
            GlowManager.Instance.ActivateGlow(buttons[index + 1]);
            currentIndex = index + 1;
        }
    }

    public void NeedButtonAgain(int index)
    {
        if (index < buttons.Length)
        {
            GlowManager.Instance.ActivateGlow(buttons[index]);
            currentIndex = index;
        }
    }
}
