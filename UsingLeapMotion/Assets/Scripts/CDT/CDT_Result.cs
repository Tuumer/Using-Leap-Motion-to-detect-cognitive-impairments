using UnityEngine;

public class CDT_Result : MonoBehaviour
{
    private MouseDraw mouseDraw;

    void Start()
    {
        mouseDraw = FindObjectOfType<MouseDraw>();
    }

    public void LogSum()
    {
        float hour = mouseDraw.GetHourResult();
        float minute = mouseDraw.GetMinuteResult();
        float sum = hour + minute;
        UnityEngine.Debug.Log("Sum: " + sum);
    }
}
