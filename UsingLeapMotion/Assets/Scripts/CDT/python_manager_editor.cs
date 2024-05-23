using UnityEngine;
using UnityEditor;
using UnityEditor.Scripting.Python;
using System.Diagnostics;
using System.IO;

[CustomEditor(typeof(python_manager))]
public class python_manager_editor : Editor
{
    python_manager targetManager;

    private void OnEnable()
    {
        targetManager = (python_manager)target;
    }

    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Launch PY Script", GUILayout.Height(35)))
        {
            string path = UnityEngine.Application.dataPath + "/T_Python/new_python_script.py"; // Specifying UnityEngine.Application

            // Start the Python process
            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = "python"; // Assumes 'python' is in PATH environment variable
            start.Arguments = path;
            start.UseShellExecute = false;
            start.RedirectStandardOutput = true;

            using (Process process = Process.Start(start))
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    string result = reader.ReadToEnd();
                    // Parse the output to get the number
                    int number;
                    if (int.TryParse(result, out number))
                    {
                        UnityEngine.Debug.Log("Output number from Python script: " + number); // Specifying UnityEngine.Debug
                    }
                    else
                    {
                        UnityEngine.Debug.LogWarning("Failed to parse output as a number: " + result); // Specifying UnityEngine.Debug
                    }
                }
            }
        }
    }
}
