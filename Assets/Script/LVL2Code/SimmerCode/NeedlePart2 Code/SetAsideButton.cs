using UnityEngine;

public class SetAsideButton : MonoBehaviour
{
    public void SetSideButton()
    {
        if (CookingStepManager.Instance != null)
        {
            Debug.Log("[Boil] Calling CookingStepManager.NextStep()");
            CookingStepManager.Instance.NextStep();
        }
        else
        {
            Debug.LogWarning("[Boil] CookingStepManager.Instance is NULL!");
        }
    }
}

