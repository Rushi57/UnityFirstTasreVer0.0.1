using UnityEngine;

public class AutoParentToPanel : MonoBehaviour
{
    private void Start()
    {
        // Check if the CookingStepManager is available
        if (CookingStepManager.Instance == null)
        {
            Debug.LogWarning("⚠️ No CookingStepManager found!");
            return;
        }

        // Get current step name (e.g. "mix", "simmer", etc.)
        string currentStep = CookingStepManager.Instance.GetExpectedStep()?.ToLower();
        if (string.IsNullOrEmpty(currentStep))
        {
            Debug.Log("⚠️ No current step to match.");
            return;
        }

        // Find panel based on step keyword
        GameObject parentPanel = null;

        switch (currentStep)
        {
            case "mix":
                parentPanel = GameObject.Find("MixPanel");
                break;
            case "saute":
                parentPanel = GameObject.Find("SautePanel");
                break;
            case "simmer":
                parentPanel = GameObject.Find("SimmerPanel");
                break;
            case "boil":
                parentPanel = GameObject.Find("BoilPanel");
                break;
            case "cut":
                parentPanel = GameObject.Find("CutPanel");
                break;
        }

        if (parentPanel != null)
        {
            transform.SetParent(parentPanel.transform, false);
            transform.SetAsLastSibling(); // bring to front if UI
            Debug.Log($"✅ {gameObject.name} is now child of {parentPanel.name}");
        }
        else
        {
            Debug.Log($"⚠️ No matching panel found for step: {currentStep}");
        }
    }
}
