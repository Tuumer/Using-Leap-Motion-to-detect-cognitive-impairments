using UnityEngine;
using UnityEngine.UI; // Required for UI elements
using System.Diagnostics;

public class CDT_Python : MonoBehaviour
{
    void Start()
    {
        // Find the button component in the scene
        Button button = GetComponent<Button>();
        // Add a listener for when the button is clicked
        button.onClick.AddListener(OnClick);
    }

    void OnClick()
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
