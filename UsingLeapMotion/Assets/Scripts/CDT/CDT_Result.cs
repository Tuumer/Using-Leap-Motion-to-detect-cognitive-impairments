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
        float hour_score, minute_score;

        if (hour > 3)
        {
            hour_score = 1;
        }
        else { 
            hour_score = 0; 
        }

        if (minute > 3)
        {
            minute_score = 1;
        }
        else
        {
            minute_score = 0;
        }

        float sum = hour_score + minute_score;
        UnityEngine.Debug.Log("CDT Result: " + sum);
    }
}
