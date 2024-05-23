using UnityEngine;
using UnityEditor;
using UnityEditor.Scripting.Python;
using System.Diagnostics;

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
            string path = Application.dataPath + "/T_Python/new_python_script.py"; // Corrected path concatenation
            PythonRunner.RunFile(path);
        }
    }
}
