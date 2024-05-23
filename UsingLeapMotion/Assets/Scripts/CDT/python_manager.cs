using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Scripting.Python;

public class python_manager : MonoBehaviour
{
    public void CNN_Python () {
        string path = Application.dataPath + "/T_Python/new_python_script.py";
        PythonRunner.RunFile(path);
    }
}
