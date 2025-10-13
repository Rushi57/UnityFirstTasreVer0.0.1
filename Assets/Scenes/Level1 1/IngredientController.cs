using UnityEngine;

public class IngredientController : MonoBehaviour
{
    [SerializeField] private GameObject tableIngredient; // The ingredient on the table

    public void MoveToTable()
    {
        if (tableIngredient != null)
        {
            tableIngredient.SetActive(true);   
            gameObject.SetActive(false);       
        }
    }
}
