using UnityEngine;

public class CutScoreManager : MonoBehaviour
{
    public static CutScoreManager Instance;

    public int choppedTotalscore { get; private set; }

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void CutAddScore(int cutAmount)
    {
        choppedTotalscore += cutAmount;
        Debug.Log($"Total Cut Score: {choppedTotalscore}");

        // ✅ Check if recipe is finished, then finalize
        if (CookingStepManager.Instance != null && CookingStepManager.Instance.IsRecipeCompleted())
        {
            TotalScoreManager.Instance.CalculateFinalScore();
        }
    }
}
