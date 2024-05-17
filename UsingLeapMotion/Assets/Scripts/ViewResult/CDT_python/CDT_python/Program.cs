using Python.Runtime;

RunScript("myddd");

static void RunScript(string scriptName)
{
    Runtime.PythonDLL = @"C:\Users\Sarah\AppData\Local\Programs\Python\Python310\python310.dll";
    PythonEngine.Initialize();
    using (Py.GIL())
    {
        var pythonScript = Py.Import(scriptName);
        //var message = new PyString("Mesage for sarah");
        var result = pythonScript.InvokeMethod("say_hello");
    }
}
