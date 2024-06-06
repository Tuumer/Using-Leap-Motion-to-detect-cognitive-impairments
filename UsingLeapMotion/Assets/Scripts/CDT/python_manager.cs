using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor.Scripting.Python;

public class python_manager : MonoBehaviour
{
    public void CNN_Python () {
        string path = Application.dataPath + "/T_Python/new_python_script.py";
        PythonRunner.RunFile(path);
    }

    public void class_predict(){

        string path = Application.dataPath + "/T_Python/classification_model.py";
        PythonRunner.RunFile(path);

    }
}
#endif
