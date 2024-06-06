using UnityEngine;
using System.IO;
using System.Collections;

public class Screenshot : MonoBehaviour
{
    public GameObject ellipseImage;
    public GameObject ellipseImage1;
    public GameObject popUp;

    private string screenshotFolder;

    private void Start()
    {
        // Define the path for the existing screenshots folder
        screenshotFolder = Path.Combine(UnityEngine.Application.dataPath, "screenshots");

        // Ensure the folder exists
        if (!Directory.Exists(screenshotFolder))
        {
            UnityEngine.Debug.LogError("The specified screenshots folder does not exist: " + screenshotFolder);
            return;
        }
    }

    // Method to capture a screenshot and manage UI elements
    public void CaptureScreenshot()
    {
        StartCoroutine(CaptureScreenshotProcess());
    }

    private IEnumerator CaptureScreenshotProcess()
    {
        // Deactivate the ellipse images before taking the screenshot
        DeactivateEllipseImages();

        // Wait a short time to ensure the images are deactivated
        yield return new WaitForEndOfFrame();

        // Create the filename for the screenshot
        string filename = Path.Combine(screenshotFolder, "aaa.png");
        UnityEngine.Debug.Log("Saving screenshot to: " + filename);

        // Capture the screenshot
        ScreenCapture.CaptureScreenshot(filename, 1); // Set scale factor to 1 for normal resolution

        // Wait for the screenshot to be captured
        yield return new WaitForEndOfFrame();

        // Reactivate the ellipse images after taking the screenshot
        ActivateEllipseImages();

        // Open the popup
        if (popUp != null)
        {
            popUp.SetActive(true);
        }
    }

    // Method to deactivate ellipse images
    public void DeactivateEllipseImages()
    {
        if (ellipseImage != null)
        {
            ellipseImage.SetActive(false);
        }

        if (ellipseImage1 != null)
        {
            ellipseImage1.SetActive(false);
        }
    }

    // Method to activate ellipse images
    public void ActivateEllipseImages()
    {
        if (ellipseImage != null)
        {
            ellipseImage.SetActive(true);
        }

        if (ellipseImage1 != null)
        {
            ellipseImage1.SetActive(true);
        }
    }
}
