using UnityEngine;
using UnityEngine.UI;

public class CDT_Result : MonoBehaviour
{
    private MouseDraw mouseDraw;
    private float hour;
    private float minute;
    private float sum;

    void Start()
    {
        mouseDraw = FindObjectOfType<MouseDraw>();

        hour = mouseDraw.GetHourResult();
        minute = mouseDraw.GetMinuteResult();

        sum = hour + minute;
    }

    public void LogSum()
    {
        UnityEngine.Debug.Log("Sum: " + sum);
    }
}
