using UnityEngine;

public class TotalScoreManager : MonoBehaviour
{
    public static TotalScoreManager Instance;

<<<<<<< Updated upstream
    private int mixScore;
    private int cutScore;
    private int simmerScore;

    public int FinalScore { get; private set; }
=======
    public GameObject finalScorePanel;

    public int finalScore { get; private set; }
>>>>>>> Stashed changes

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // 🔹 Register scores instead of having separate managers
    public void AddMixScore(int amount)
    {
        mixScore += amount;
        Debug.Log($"Mix Score Added: {amount} | Current Total Mix: {mixScore}");
    }

<<<<<<< Updated upstream
    public void AddCutScore(int amount)
    {
        cutScore += amount;
        Debug.Log($"Cut Score Added: {amount} | Current Total Cut: {cutScore}");
    }

    public void AddSimmerScore(int amount)
    {
        simmerScore += amount;
        Debug.Log($"Simmer Score Added: {amount} | Current Total Simmer: {simmerScore}");
    }

    // 🔹 Called once when recipe is done
    public void CalculateFinalScore(string recipeName)
    {
        FinalScore = mixScore + cutScore + simmerScore;

        Debug.Log($"📊 Final Breakdown → Mix:{mixScore}, Cut:{cutScore}, Simmer:{simmerScore}");
        Debug.Log($"🏆 Total Score: {FinalScore}");
        Debug.Log($"✅ Recipe {recipeName} Completed!");
    }

    // 🔹 Optional reset between recipes
    public void ResetScores()
    {
        mixScore = 0;
        cutScore = 0;
        simmerScore = 0;
        FinalScore = 0;
=======
        finalScore = mix + cut + simmer;
        Debug.Log($"🍲 Final Score for Recipe: {finalScore} (Mix:{mix}, Cut:{cut}, Simmer:{simmer})");
>>>>>>> Stashed changes
    }

    public void CheckIfAllFinished()
    {
        bool mixDone = MixingScoreManager.Instance == null || MixingScoreManager.Instance.isFinished;
        bool cutDone = CutScoreManager.Instance == null || CutScoreManager.Instance.isFinished;
        bool simmerDone = SimmerScoreManager.Instance == null || SimmerScoreManager.Instance.isFinished;

        if (mixDone && cutDone && simmerDone)
        {
            Debug.Log("🎉 All steps finished, calculating final score...");
            CalculateFinalScore();
            // ✅ Show final score panel here if you want
            if (CookingStepManager.Instance != null)
                CookingStepManager.Instance.OnRecipeComplete();
        }
    }
}
