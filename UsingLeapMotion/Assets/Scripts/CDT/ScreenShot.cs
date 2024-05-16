using UnityEngine;
using UnityEngine.UI;
using System;

public class Screenshot : MonoBehaviour
{
    public Button screenshotButton; // Reference to the button in the Inspector
    public GameObject popUp;

    private void Start()
    {
        // Check if the screenshotButton is assigned
        if (screenshotButton != null)
        {
            // Add a listener to the button's onClick event
            screenshotButton.onClick.AddListener(CaptureScreenshot);
        }
        else
        {
            UnityEngine.Debug.LogError("Screenshot Button is not assigned!");
        }
    }

    private void CaptureScreenshot()
    {
        ScreenCapture.CaptureScreenshot("screenshot-" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".png", 4);

        if (popUp != null)
        {
            Invoke("ActivatePopUp", 0.5f);
        }
        else
        {
            UnityEngine.Debug.LogError("PopUP GameObject is not assigned!");
        }
    }

    private void ActivatePopUp()
    {
        // Set PopUP GameObject active
        popUp.SetActive(true);
    }
}
