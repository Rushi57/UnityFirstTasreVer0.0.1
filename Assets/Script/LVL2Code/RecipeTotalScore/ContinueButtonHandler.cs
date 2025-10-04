using UnityEngine;
using UnityEngine.SceneManagement;

public class ContinueButtonHandler : MonoBehaviour
{
    public string mapSceneName = "Map";
    public RecipeSO currentRecipe;

    public void OnContinueButton()
    {
        //Save score before Leaving
        TotalScoreManager.Instance.CalculateFinalScore("Level1", currentRecipe);

        //Reset Level1  state if need 
        ResetLevelProgress();


        //Load Map scene
        SceneManager.LoadScene(mapSceneName);
    }

    void ResetLevelProgress()
    {
        //Reset score Variable
        TotalScoreManager.Instance.ResetScores();

        //Rese Inventory
        Debug.Log("Level progress reset");
    }

}
