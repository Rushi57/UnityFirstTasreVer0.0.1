using System;
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

    public bool IsOnFire => isChanged;
    public static event Action<bool> OnFireToggle;
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

            foreach (var heat in FindObjectsByType<PanHeatEffect>(FindObjectsSortMode.None))
                heat.StopHeating();

            // Notify listeners
            OnFireToggle?.Invoke(false);
        }
        else
        {
            // change to new image
            button.image.sprite = newButtonImage;
            isChanged = true;

            // Notify listeners
            OnFireToggle?.Invoke(true);
        }
    }
}
