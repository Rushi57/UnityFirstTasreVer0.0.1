using UnityEngine;
using UnityEngine.EventSystems;

public class PanMixngHandler : MonoBehaviour, IDropHandler
{
    private Animator animator;

    [Header("MixingPanel")]
    public GameObject mixingMechPanel;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        ItemData droppedItem = eventData.pointerDrag.GetComponent<ItemData>();
        if (droppedItem == null || droppedItem.itemSO == null) return;

        ItemSO itemSO = droppedItem.itemSO;


        //CheckIfSpatula
        if (itemSO.itemType == ItemType.Utility && itemSO.itemName == "Spatula")
        {
            Debug.Log("Spatula dropped in Pan");


            //Confirmation in CookingStepManager
            CookingStepManager.Instance.OnActionPerformed("Mixing");


            //PlayAnimation
            if (animator != null)
                animator.SetTrigger("Mixing");


            if(mixingMechPanel != null)
                mixingMechPanel.SetActive(true);


            //Removed Spatula Form the table
            Destroy(eventData.pointerDrag);
        }
        else
        {
            CookingStepManager.Instance.WrongAttempt();

            Draggable drag = droppedItem.GetComponent<Draggable>();
            if (drag != null)
                drag.RevertToOriginalPosition();
        }
    }
}
