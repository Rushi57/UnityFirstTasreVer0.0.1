using UnityEngine;

public class MixingScoreManager : MonoBehaviour
{
    public static MixingScoreManager Instance;

    public int totalMixScore { get; private set; }

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddMixingScore(int mixAmount)
    {
        totalMixScore += mixAmount;
        Debug.Log($"Total Mixing Score: {totalMixScore}");

        // ✅ Check if recipe is finished, then finalize
        if (CookingStepManager.Instance != null && CookingStepManager.Instance.IsRecipeCompleted())
        {
            TotalScoreManager.Instance.CalculateFinalScore();
        }
    }
}
