using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeImage : MonoBehaviour
{
    public Sprite newButtonImage;
    public Button button;

    private Sprite oldButtonImage; // store the original image
    private bool isChanged = false; // track if changed or not

    void Start()
    {
        // store the button's current sprite at start
        oldButtonImage = button.image.sprite;
    }

    public void ChangeButtonImage()
    {
        if (isChanged)
        {
            // revert back to old image
            button.image.sprite = oldButtonImage;
            isChanged = false;
        }
        else
        {
            // change to new image
            button.image.sprite = newButtonImage;
            isChanged = true;
        }
    }
}
