using UnityEngine;

public class SimmerScoreManager : MonoBehaviour
{
    public static SimmerScoreManager Instance { get; private set; }

    public int simmerTotalScore { get; private set; }

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void SimmerAddScore(int simmerAmount)
    {
        simmerTotalScore += simmerAmount;
        Debug.Log($"Total Simmer Score: {simmerTotalScore}");

        // ✅ Check if recipe is finished, then finalize
        if (CookingStepManager.Instance != null && CookingStepManager.Instance.IsRecipeCompleted())
        {
            TotalScoreManager.Instance.CalculateFinalScore();
        }
    }
}
