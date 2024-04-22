using UnityEngine;
using UnityEngine.UI;
using System;

public class Screenshot : MonoBehaviour
{
    public Button screenshotButton; // Reference to the button in the Inspector

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
    }
}
