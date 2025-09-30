using UnityEngine;

public class CutScoreManager : MonoBehaviour
{
    public static CutScoreManager Instance;

    public int choppedTotalscore { get; private set; }
    public int targetCutScore = 10; // set in Inspector per recipe/mini-game
    public bool isFinished { get; private set; } = false; // ✅ added flag

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void CutAddScore(int cutAmount)
    {
        choppedTotalscore += cutAmount;
        Debug.Log($"Total Cut Score: {choppedTotalscore}");
    }
}
