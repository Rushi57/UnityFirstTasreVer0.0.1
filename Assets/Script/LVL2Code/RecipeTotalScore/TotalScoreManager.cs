using UnityEngine;

public class TotalScoreManager : MonoBehaviour
{
    public static TotalScoreManager Instance;

    private int mixScore;
    private int cutScore;
    private int simmerScore;

    public int FinalScore { get; private set; }

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
    }
}
