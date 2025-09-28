using UnityEngine;

public class CondimentAction : MonoBehaviour
{
    [SerializeField] private string actionName; // e.g. "Vinegar", "Oil", "Soy"

    // 👇 This will be called from the Animation Event
    public void PerformAction()
    {
        if (CookingStepManager.Instance != null)
        {
            CookingStepManager.Instance.OnActionPerformed(actionName);
        }

        // Hide bottle after pour
        gameObject.SetActive(false);
    }
}
