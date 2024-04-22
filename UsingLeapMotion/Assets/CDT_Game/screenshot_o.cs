using UnityEngine;
using System;
 
public class Screenshot : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            ScreenCapture.CaptureScreenshot("screenshot-" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".png", 4);
        }
    }
}