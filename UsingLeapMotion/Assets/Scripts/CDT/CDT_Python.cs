using UnityEngine;
using UnityEngine.UI;
using System.Diagnostics;

public class CDT_Python : MonoBehaviour
{
    // Reference to the button object in the Unity Editor
    public Button button;

    void Start()
    {
        // Check if the button reference is assigned
        if (button != null)
        {
            // Add a listener for when the button is clicked
            button.onClick.AddListener(OnClick);
        }
        else
        {
            UnityEngine.Debug.LogError("Button reference is not assigned. Please assign it in the Unity Editor.");
        }
    }

    public void OnClick()
    {
        // Path to your Python script
        string pythonScriptPath = @"C:\Users\Sarah\Documents\GitHub\Using-Leap-Motion-to-detect-cognitive-impairments\UsingLeapMotion\Assets\new_python_script.py";

        // Create process info
        ProcessStartInfo start = new ProcessStartInfo();
        start.FileName = "python"; // Python interpreter path
        start.Arguments = pythonScriptPath;
        start.UseShellExecute = false;
        start.RedirectStandardOutput = true;

        // Start the process
        using (Process process = Process.Start(start))
        {
            // Read the output from the Python script
            using (System.IO.StreamReader reader = process.StandardOutput)
            {
                string result = reader.ReadToEnd();
                int trueCount;
                if (int.TryParse(result, out trueCount))
                {
                    // Log the result
                    UnityEngine.Debug.Log("Number of true conditions: " + trueCount);
                }
                else
                {
                    // Log an error if parsing fails
                    UnityEngine.Debug.LogError("Failed to parse result: " + result);
                }
            }
        }
    }
}
