using UnityEngine;

public class MixingScoreManager : MonoBehaviour
{
    public static MixingScoreManager Instance;

    public int totalMixScore { get; private set; }
    public int targetMixingScore = 10; // set in Inspector per recipe/mini-game
    public bool isFinished { get; private set; } = false; // ✅ added flag

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddMixingScore(int mixAmount)
    {
        totalMixScore += mixAmount;
        Debug.Log($"Total Mixing Score: {totalMixScore}");

        if (totalMixScore >= targetMixingScore && !isFinished)
        {
            isFinished = true;
            Debug.Log("✅ Mixing finished!");
            TotalScoreManager.Instance.CheckIfAllFinished();
        }
    }
}
