using UnityEngine;
using UnityEditor;
using UnityEditor.Scripting.Python;

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
            string path = Application.dataPath + "/T_Python/new_python_script.py";
            string pers_path= Application.persistentDataPath + "/T_Python/new_python_script.py";
            Debug.Log("Current data path:" + path);
            Debug.Log("Persistent data path:" + pers_path);
            PythonRunner.RunFile(path);
        }
    }
}
