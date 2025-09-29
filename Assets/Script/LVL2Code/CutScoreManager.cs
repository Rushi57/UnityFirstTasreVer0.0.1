using UnityEngine;

public class CutScoreManager : MonoBehaviour
{
    public static CutScoreManager Instance;

    [Header("Scores")]
    public int choppedTotalscore = 0;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddCutScore(int amount)
    {
        choppedTotalscore += amount;
    }

    public void ResetCutScore()
    {
        choppedTotalscore = 0;
    }
}
