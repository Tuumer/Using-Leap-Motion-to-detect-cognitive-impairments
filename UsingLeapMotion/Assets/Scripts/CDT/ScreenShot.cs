using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;

public class Screenshot : MonoBehaviour
{
    public Button screenshotButton;
    public GameObject popUp;
    public GameObject ellipseImage;
    public GameObject ellipseImage1;
    private string screenshotFolder;

    private void Start()
    {
        // Define the path for the existing screenshots folder
        screenshotFolder = Path.Combine(UnityEngine.Application.dataPath, "..", "screenshots");

        // Ensure the folder exists
        if (!Directory.Exists(screenshotFolder))
        {
            UnityEngine.Debug.LogError("The specified screenshots folder does not exist: " + screenshotFolder);
            return;
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

        if(ellipseImage1 != null)
        {
            ellipseImage1.SetActive(false);
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

        if (ellipseImage1 != null)
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
        Invoke("DeactivateEllipseImage", 100.0f); // Deactivate the ellipse image after 2 seconds
    }

    private void DeactivateEllipseImage()
    {
        ellipseImage.SetActive(false);
    }

    private void ActivateEllipseImage1()
    {
        ellipseImage1.SetActive(true);
        Invoke("DeactivateEllipseImage", 100.0f); // Deactivate the ellipse image after 2 seconds
    }

    private void DeactivateEllipseImage1()
    {
        ellipseImage1.SetActive(false);
    }

    private void ActivatePopUp()
    {
        popUp.SetActive(true);
    }
}