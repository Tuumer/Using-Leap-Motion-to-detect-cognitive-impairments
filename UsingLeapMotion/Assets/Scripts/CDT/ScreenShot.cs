using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;

public class Screenshot : MonoBehaviour
{
    public Button screenshotButton;
    public GameObject popUp;
    public GameObject ellipseImage;
    private string screenshotFolder;

    private void Start()
    {
        // Define the path for the screenshots folder
        screenshotFolder = Path.Combine(UnityEngine.Application.persistentDataPath, "screenshots");

        // Ensure the folder exists
        if (!Directory.Exists(screenshotFolder))
        {
            Directory.CreateDirectory(screenshotFolder);
        }

        // Set up the screenshot button listener
        if (screenshotButton != null)
        {
            screenshotButton.onClick.AddListener(CaptureScreenshot);
        }
        else
        {
            UnityEngine.Debug.LogError("Screenshot Button is not assigned!");
        }
    }

    private void CaptureScreenshot()
    {
        // Deactivate the ellipse image before taking the screenshot
        if (ellipseImage != null)
        {
            ellipseImage.SetActive(false);
        }

        // Create the filename for the screenshot
        string filename = Path.Combine(screenshotFolder, "screenshot-" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".png");
        UnityEngine.Debug.Log("Saving screenshot to: " + filename);

        // Capture the screenshot
        ScreenCapture.CaptureScreenshot(filename, 1); // Set scale factor to 1 for normal resolution

        // Reactivate the ellipse image after taking the screenshot
        if (ellipseImage != null)
        {
            Invoke("ActivateEllipseImage", 0.5f); // Adjust the delay as needed
        }

        // Activate the pop-up if it is assigned
        if (popUp != null)
        {
            Invoke("ActivatePopUp", 0.5f);
        }
        else
        {
            UnityEngine.Debug.LogError("PopUP GameObject is not assigned!");
        }
    }

    private void ActivateEllipseImage()
    {
        ellipseImage.SetActive(true);
        Invoke("DeactivateEllipseImage", 2.0f); // Deactivate the ellipse image after 2 seconds
    }

    private void DeactivateEllipseImage()
    {
        ellipseImage.SetActive(false);
    }

    private void ActivatePopUp()
    {
        popUp.SetActive(true);
    }
}
