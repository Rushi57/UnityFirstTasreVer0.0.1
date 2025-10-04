using UnityEngine;

public class LevelResetManager : MonoBehaviour
{
    void Start()
    {
        if (TotalScoreManager.Instance != null)
        {
            TotalScoreManager.Instance.ResetScores();
        }

        // Also reset other per-run systems: Inventory, chopped states, timers, etc.
        // e.g. IngredientManager.Instance.ResetAll();
    }
}
