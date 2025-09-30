using UnityEngine;

public class TotalScoreManager : MonoBehaviour
{
    public static TotalScoreManager Instance;

    public int finalScore { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void CalculateFinalScore()
    {
        int mix = MixingScoreManager.Instance != null ? MixingScoreManager.Instance.totalMixScore : 0;
        int cut = CutScoreManager.Instance != null ? CutScoreManager.Instance.choppedTotalscore : 0;
        int simmer = SimmerScoreManager.Instance != null ? SimmerScoreManager.Instance.simmerTotalScore : 0;

        finalScore = mix + cut + simmer;

        Debug.Log($"🍲 Final Score for Recipe: {finalScore} (Mix:{mix}, Cut:{cut}, Simmer:{simmer})");
    }
}
