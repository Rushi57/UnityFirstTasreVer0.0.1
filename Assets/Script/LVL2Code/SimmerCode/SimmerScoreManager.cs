using UnityEngine;

public class SimmerScoreManager : MonoBehaviour
{
    public static SimmerScoreManager Instance { get; private set; }

    public int simmerTotalScore { get; private set; }
    public int targetSimmerScore = 10; // set in Inspector per recipe/mini-game
    public bool isFinished { get; private set; } = false; // ✅ added flag

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void SimmerAddScore(int simmerAmount)
    {
        simmerTotalScore += simmerAmount;
        Debug.Log($"Total Simmer Score: {simmerTotalScore}");

        if (simmerTotalScore >= targetSimmerScore && !isFinished)
        {
            isFinished = true;
            Debug.Log("✅ Simmering finished!");
            TotalScoreManager.Instance.CheckIfAllFinished();
        }
    }
}
