using Python.Runtime;

static void RunScript(string scriptName)
{
    Runtime.PythonDLL = @"C:\Users\Sarah\AppData\Local\Programs\Python\Python310\python310.dll";
    PythonEngine.Initialize();
    using (Py.GIL())
    {
        var pythonScript = Py.Import("my")
    }
}