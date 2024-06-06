using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Scripting.Python;
#endif

public class python_manager : MonoBehaviour
{
    public void CNN_Python()
    {
#if UNITY_EDITOR
        string path = Application.dataPath + "/T_Python/new_python_script.py";
        PythonRunner.RunFile(path);
#else
        UnityEngine.Debug.LogWarning("Python scripts can only be run in the Unity Editor.");
#endif
    }

    public void class_predict()
    {
#if UNITY_EDITOR
        string path = Application.dataPath + "/T_Python/classification_model.py";
        PythonRunner.RunFile(path);
#else
        UnityEngine.Debug.LogWarning("Python scripts can only be run in the Unity Editor.");
#endif
    }
}
