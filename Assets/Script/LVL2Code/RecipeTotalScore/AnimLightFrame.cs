using UnityEngine;

public class CongratsTriggerAnim : MonoBehaviour
{
    [Header("References")]
    public GameObject animLightFrame;      // assign your AnimLightFrame here
    private bool wasActive = false;        // tracks last state

    void Update()
    {
        // Check if CongratsDisplayPanel is active
        bool isActive = gameObject.activeInHierarchy;

        // When it becomes active (trigger only once)
        if (isActive && !wasActive)
        {
            if (animLightFrame != null)
            {
                animLightFrame.SetActive(true);
                Animator anim = animLightFrame.GetComponent<Animator>();
                if (anim != null)
                {
                    anim.Play("AnimLightFrame_Anim", -1, 0f);
                }
            }
        }

        // Optionally hide when CongratsDisplayPanel turns off
        if (!isActive && wasActive)
        {
            if (animLightFrame != null)
                animLightFrame.SetActive(false);
        }

        // Update last known state
        wasActive = isActive;
    }
}
